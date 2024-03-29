﻿namespace HomeSimulator.Services;

using HomeSimulator.Web.Extensions;
using HomeSimulator.Web.Helpers;
using HomeSimulator.Web.Models;
using HomeSimulator.Web.Models.Entities;
using HomeSimulator.Web.Models.OCPP;
using Newtonsoft.Json;

using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;


public class OCPPSocketBackgroundService /*: IHostedService, IDisposable*/
{
    ClientWebSocket Client = new ClientWebSocket();
    public Charger Charger = new Charger();
    private static string MessageRegExp = "^\\[\\s*(\\d)\\s*,\\s*\"([^\"]*)\"\\s*,(?:\\s*\"(\\w*)\"\\s*,)?\\s*(.*)\\s*\\]$";
    public Dictionary<string, OCPPMessage> _requestQueue = new Dictionary<string, OCPPMessage>();
    NLog.Logger logger = NLog.LogManager.GetLogger("ocpp");

    public void Dispose()
    {
        if (Client.State == WebSocketState.Open)
            Client.CloseAsync(WebSocketCloseStatus.NormalClosure, "shutdown", new CancellationToken());
        Client.Dispose();
    }

    System.Threading.Timer? HearbeatTimer;
    System.Threading.Timer? MetervaluesTimer;

    public bool IsConnected
    {
        get
        {
            return Client != null && Client.State == WebSocketState.Open;
        }
    }

    private readonly IServiceProvider _provider;

    public OCPPSocketBackgroundService(IServiceProvider provider)
    {
        HearbeatTimer = new System.Threading.Timer(Hearbeat, null, TimeSpan.FromSeconds(2), TimeSpan.FromMinutes(1));
        //Charger = new Charger() { MeterStart = 1, OCPPServer = "ws://localhost:8081/OCPP", ChargePointId = "Test1234", IdTag = "630E87A6" };
        _provider = provider;

        GetChargerFromDB();
    }

    void GetChargerFromDB()
    {
        using (var scope = _provider.CreateScope())
        {
            var myContext = scope.ServiceProvider.GetRequiredService<MyContext>();
            Charger = myContext.Chargers.First();
        }
    }

    void SaveChargerToDB()
    {
        using (var scope = _provider.CreateScope())
        {
            var myContext = scope.ServiceProvider.GetRequiredService<MyContext>();
            var dbCharger = myContext.Chargers.First();
            dbCharger.LimitPower = Charger.LimitPower;
            dbCharger.LimitPowerExpiredTime = Charger.LimitPowerExpiredTime;
            dbCharger.LastUpdateTime = Charger.LastUpdateTime;
            dbCharger.Voltage = Charger.Voltage;
            dbCharger.Current = Charger.Current;
            dbCharger.Power = Charger.Power;
            dbCharger.Energy = Charger.Energy;
            dbCharger.SOC = Charger.SOC;
            dbCharger.TransactionId = Charger.TransactionId;
            dbCharger.MeterStart = Charger.MeterStart;
            dbCharger.MeterStartTime = Charger.MeterStartTime;
            dbCharger.MeterStop = Charger.MeterStop;
            dbCharger.MeterStopTime = Charger.MeterStopTime;
            myContext.SaveChanges();
        }
    }

    public void StartAsync()
    {
        GetChargerFromDB();
        if (string.IsNullOrEmpty(Charger.ChargePointId) || string.IsNullOrEmpty(Charger.OCPPServer))
            return;

        var Url = $"{Charger.OCPPServer}/{Charger.ChargePointId}";

        if (Client.State == WebSocketState.Open) return;

        //var ocppServer = "ws://localhost:8081/OCPP";
        //var chargePointId = "Test1234";
        // 连接ocppserver


        Task.Run(async () =>
        {
            try
            {
                Client = new ClientWebSocket();

                Client.Options.AddSubProtocol("ocpp1.6");

                await Client.ConnectAsync(new Uri(Url), CancellationToken.None);


                MetervaluesTimer = new System.Threading.Timer(Metervalues, null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));

                var timer = new System.Timers.Timer(1000); // 延时1秒
                timer.Elapsed += (sender, e) =>
                {
                    _ = InitCharger();
                };
                timer.AutoReset = false; // 设置为不重复执行
                timer.Start();


                // 发送bootnotification
                //await SendBootNotification();

                // 循环读取
                while (Client.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    var result = await Client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result != null && result.MessageType != WebSocketMessageType.Close)
                    {
                        // 处理消息
                        var ocppMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        logger.Info("receive:" + ocppMessage);

                        Match match = Regex.Match(ocppMessage, MessageRegExp);
                        if (match != null && match.Groups != null && match.Groups.Count >= 3)
                        {
                            string messageTypeId = match.Groups[1].Value;
                            string uniqueId = match.Groups[2].Value;
                            string action = match.Groups[3].Value;
                            string jsonPaylod = match.Groups[4].Value;

                            OCPPMessage msgIn = new OCPPMessage(messageTypeId, uniqueId, action, jsonPaylod);
                            if (msgIn.MessageType == "2")
                            {
                                OCPPMessage msgOut = ProcessRequest(msgIn);

                                await SendMessage(msgOut);
                            }
                            else if (msgIn.MessageType == "3" || msgIn.MessageType == "4")
                            {
                                if (_requestQueue.ContainsKey(msgIn.UniqueId))
                                {
                                    ProcessAnswer(msgIn, _requestQueue[msgIn.UniqueId]);
                                    _requestQueue.Remove(msgIn.UniqueId);
                                }
                                else
                                {
                                    Console.WriteLine("OCPPMiddleware.Receive16 => HttpContext from caller not found / Msg: {0}", ocppMessage);
                                }
                            }
                            else
                            {
                                // Unknown message type
                                Console.WriteLine("OCPPMiddleware.Receive16 => Unknown message type: {0} / Msg: {1}", msgIn.MessageType, ocppMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine("OCPPMiddleware.Receive16 => Error in RegEx-Matching: Msg={0})", ocppMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        });

    }

    private void Hearbeat(object? state)
    {
        var r = ConfirmConnect();
        if (r == false) return;

        var request = new HeartbeatRequest();

        string jsonResetRequest = JsonConvert.SerializeObject(request);

        OCPPMessage msgOut = new OCPPMessage();
        msgOut.MessageType = "2";
        msgOut.Action = "Heartbeat";
        msgOut.UniqueId = Guid.NewGuid().ToString("N");
        msgOut.JsonPayload = jsonResetRequest;
        msgOut.TaskCompletionSource = new TaskCompletionSource<string>();

        _requestQueue.Add(msgOut.UniqueId, msgOut);

        _ = SendMessage(msgOut);

    }

    private void Metervalues(object? state)
    {
        // Check the current status. If it is charging, send metervalues
        if (Charger.IsCharging)
        {
            UpdateChargerData();
            SaveChargerToDB();
            _ = SendMeterValues();
        }
    }

    #region handle charger data

    public void StartTransaction()
    {
        // if stop has value, use it as start
        if (Charger.MeterStop != null)
        {
            Charger.MeterStart = Charger.MeterStop;
        }
        else
        {
            Charger.MeterStart = 0;
        }
        Charger.MeterStartTime = DateTimeOffset.Now;
        // clear data
        Charger.LastUpdateTime = null;
        Charger.Voltage = null;
        Charger.Current = null;
        Charger.Power = null;
        Charger.Energy = Charger.MeterStart;
        Charger.SOC = null;
        //Charger.LimitPower = null;
        Charger.LimitPowerExpiredTime = null;
        Charger.MeterStop = null;
        Charger.MeterStopTime = null;
        SaveChargerToDB();
        _ = SendStartTransaction();
        _ = SendStatusNotification();
        var timer = new System.Threading.Timer(Metervalues, null, 2000, Timeout.Infinite);

    }

    public void StopTransaction(bool needUpdate = true)
    {
        if (needUpdate)
            UpdateChargerData();
        Charger.MeterStopTime = DateTimeOffset.Now;
        Charger.MeterStop = (int?)Charger.Energy;
        SaveChargerToDB();
        _ = SendStopTransaction();
        _ = SendStatusNotification();
    }

    /// <summary>
    /// random power, caculate others ,update to db
    /// </summary>
    void UpdateChargerData()
    {
        var now = DateTime.Now;
        var maxPower = Charger.MaxPower ?? 70000;
        if (Charger.SOC > 90)
        {
            maxPower = maxPower * 0.3m;
        }
        if (Charger.IsPowerLimiting && Charger.LimitPower < maxPower)
        {
            maxPower = Charger.LimitPower.Value;
        }
        var randomPower = maxPower - CommonHelper.GetRandomNumber(0, 50);
        if(randomPower<0) randomPower = 0;
        var randomVoltage = 230;// CommonHelper.GetRandomNumber(220 * 0.98m, 220 * 1.02m);
        var energy = 0m;

        var avgPower = maxPower;
        if (Charger.LastUpdateTime != null && Charger.Power != null)
        {
            var seconds = (decimal)(now - Charger.LastUpdateTime.Value).TotalSeconds;
            avgPower = (Charger.Power.Value + randomPower) / 2;
            energy = avgPower * seconds / 3600m;
        }
        // soc
        if (Charger.SOC != null && Charger.SOC > 0)
        {
            // energy Wh,maxenergy kWh
            var soc = Charger.SOC.Value + energy * 0.1m / Charger.MaxEnergy;
            if (soc > 100)
            {
                soc = 100;
            }
            Charger.SOC = soc;
        }
        else
        {
            Charger.SOC = CommonHelper.GetRandomNumber(20, 60);
        }
        // set now
        Charger.LastUpdateTime = now;
        Charger.Voltage = randomVoltage;
        Charger.Current = randomPower / randomVoltage;
        Charger.Power = randomPower;
        Charger.Energy = (Charger.Energy ?? 0) + energy;
        if (Charger.SOC >= 100)
            StopTransaction(false);
    }

    #endregion

    async Task InitCharger()
    {
        await SendBootNotification();
        await SendStatusNotification();
    }

    bool ConfirmConnect()
    {
        if (Client.State != WebSocketState.Open)
            StartAsync();
        Thread.Sleep(2000);
        return Client.State == WebSocketState.Open;
    }

    #region handle answer

    private void ProcessAnswer(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        switch (msgOut.Action)
        {
            case "MeterValues":
                HandleMeterValues(msgIn, msgOut);
                break;
            case "BootNotification":
                HandleBootNotification(msgIn, msgOut);
                break;
            case "StatusNotification":
                HandleStatusNotification(msgIn, msgOut);
                break;
            case "StartTransaction":
                HandleStartTransaction(msgIn, msgOut);
                break;
            case "StopTransaction":
                HandleStopTransaction(msgIn, msgOut);
                break;
            //case "GetConfiguration":
            //    HandleGetConfiguration(msgIn, msgOut);
            //    break;
            //case "SetChargingProfile":
            //    HandleSetChargingProfile(msgIn, msgOut);
            //    break;
            //case "UnlockConnector":
            //    HandleUnlockConnector(msgIn, msgOut);
            //    break;

            default:
                break;
        }
    }

    private void HandleMeterValues(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        try
        {
            var response = JsonConvert.DeserializeObject<MeterValuesResponse>(msgIn.JsonPayload);

            if (msgOut.TaskCompletionSource != null)
            {
                // Set API response as TaskCompletion-result
                //string apiResult = "{\"status\": " + JsonConvert.ToString(response.Status.ToString()) + "}";

                msgOut.TaskCompletionSource.SetResult("ok");
            }
        }
        catch (Exception exp)
        {
            // log
        }
    }

    private void HandleBootNotification(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        try
        {
            var response = JsonConvert.DeserializeObject<BootNotificationResponse>(msgIn.JsonPayload);

            if (msgOut.TaskCompletionSource != null)
            {
                // Set API response as TaskCompletion-result
                // string apiResult = "{\"status\": " + JsonConvert.ToString(response.Status.ToString()) + "}";

                msgOut.TaskCompletionSource.SetResult(msgIn.JsonPayload);
            }
        }
        catch (Exception exp)
        {
            // log
        }
    }

    private void HandleStatusNotification(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        try
        {
            var response = JsonConvert.DeserializeObject<StatusNotificationResponse>(msgIn.JsonPayload);

            if (msgOut.TaskCompletionSource != null)
            {
                // Set API response as TaskCompletion-result
                // string apiResult = "{\"status\": " + JsonConvert.ToString(response.Status.ToString()) + "}";

                msgOut.TaskCompletionSource.SetResult(msgIn.JsonPayload);
            }
        }
        catch (Exception exp)
        {
            // log
        }
    }


    private void HandleStartTransaction(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        try
        {
            var response = JsonConvert.DeserializeObject<StartTransactionResponse>(msgIn.JsonPayload);
            Charger.TransactionId = response.TransactionId;
            if (msgOut.TaskCompletionSource != null)
            {
                // Set API response as TaskCompletion-result
                // string apiResult = "{\"status\": " + JsonConvert.ToString(response.Status.ToString()) + "}";

                msgOut.TaskCompletionSource.SetResult(msgIn.JsonPayload);
            }
        }
        catch (Exception exp)
        {
            // log
        }
    }

    private void HandleStopTransaction(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        try
        {
            var response = JsonConvert.DeserializeObject<StopTransactionResponse>(msgIn.JsonPayload);
            if (msgOut.TaskCompletionSource != null)
            {
                // Set API response as TaskCompletion-result
                // string apiResult = "{\"status\": " + JsonConvert.ToString(response.Status.ToString()) + "}";

                msgOut.TaskCompletionSource.SetResult(msgIn.JsonPayload);
            }
        }
        catch (Exception exp)
        {
            // log
        }
    }

    #endregion

    #region handle request

    private OCPPMessage ProcessRequest(OCPPMessage msgIn)
    {
        OCPPMessage msgOut = new OCPPMessage();
        msgOut.MessageType = "3";
        msgOut.UniqueId = msgIn.UniqueId;

        string? errorCode = null;

        switch (msgIn.Action)
        {
            case "GetConfiguration":
                errorCode = HandleGetConfiguration(msgIn, msgOut);
                break;
            case "ChangeConfiguration":
                errorCode = HandleChangeConfiguration(msgIn, msgOut);
                break;
            case "ChangeAvailability":
                errorCode = HandleChangeAvailability(msgIn, msgOut);
                break;
            case "SetChargingProfile":
                errorCode = HandleSetChargingProfile(msgIn, msgOut);
                break;
            case "RemoteStartTransaction":
                errorCode = HandleRemoteStartTransaction(msgIn, msgOut);
                break;
            case "RemoteStopTransaction":
                errorCode = HandleRemoteStopTransaction(msgIn, msgOut);
                break;
            case "UnlockConnector":
                errorCode = HandleUnlockConnector(msgIn, msgOut);
                break;
            case "Reset":
                errorCode = HandleReset(msgIn, msgOut);
                break;
            default:
                errorCode = ErrorCodes.NotSupported;
                break;
        }

        if (!string.IsNullOrEmpty(errorCode))
        {
            // Inavlid message type => return type "4" (CALLERROR)
            msgOut.MessageType = "4";
            msgOut.ErrorCode = errorCode;
        }

        return msgOut;
    }

    private string? HandleReset(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        string errorCode = null;

        var response = new ResetResponse();
        response.Status = ResetResponseStatus.Accepted;

        msgOut.JsonPayload = JsonConvert.SerializeObject(response);

        return errorCode;
    }

    private string? HandleUnlockConnector(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        string errorCode = null;

        var response = new UnlockConnectorResponse();
        response.Status = UnlockConnectorResponseStatus.Unlocked;

        msgOut.JsonPayload = JsonConvert.SerializeObject(response);

        return errorCode;
    }

    private string? HandleRemoteStopTransaction(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        string errorCode = null;

        var response = new RemoteStopTransactionResponse();
        response.Status = RemoteStopTransactionResponseStatus.Accepted;

        msgOut.JsonPayload = JsonConvert.SerializeObject(response);

        var timer = new Timer((object? obj) => { StopTransaction(); }, null, 2000, Timeout.Infinite);

        return errorCode;
    }

    private string? HandleRemoteStartTransaction(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        string errorCode = null;

        var response = new RemoteStartTransactionResponse();
        response.Status = RemoteStartTransactionResponseStatus.Accepted;

        msgOut.JsonPayload = JsonConvert.SerializeObject(response);

        var timer = new Timer((object? obj) => { StartTransaction(); }, null, 2000, Timeout.Infinite);

        return errorCode;
    }

    private string? HandleChangeConfiguration(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        string errorCode = null;

        var response = new ChangeConfigurationResponse();
        response.Status = ChangeConfigurationResponseStatus.Accepted;

        msgOut.JsonPayload = JsonConvert.SerializeObject(response);
        return errorCode;
    }

    private string? HandleChangeAvailability(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        string errorCode = null;
        var request = JsonConvert.DeserializeObject<ChangeAvailabilityRequest>(msgIn.JsonPayload);
        if (request != null)
        {
            if (request.Type == ChangeAvailabilityRequestType.Operative)
            {

            }
            else
            {

            }
        }
        var response = new ChangeAvailabilityResponse();
        response.Status = ChangeAvailabilityResponseStatus.Scheduled;

        msgOut.JsonPayload = JsonConvert.SerializeObject(response);
        return errorCode;
    }

    private string? HandleGetConfiguration(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        string errorCode = null;
        var request = JsonConvert.DeserializeObject<GetConfigurationRequest>(msgIn.JsonPayload);
        var response = new GetConfigurationResponse();
        var list = new List<ConfigurationKey>() {
         new ConfigurationKey{ Key="ChargingScheduleAllowedChargingRateUnit", Value="Current", Readonly=true },
         new ConfigurationKey{ Key="SupportedFeatureProfiles", Value="Core,FirmwareManagement,LocalAuthListManagement,RemoteTrigger,Reservation,SmartCharging",Readonly=true },
         new ConfigurationKey{ Key="NumberOfConnectors", Value="1",Readonly=true },
         new ConfigurationKey{ Key="HeartbeatInterval", Value="60",Readonly=true },
         new ConfigurationKey{Key="AuthorizeRemoteTxRequests",Value="true", Readonly=false},
         new ConfigurationKey{ Key="ChargeProfileMaxStackLevel", Value="20",Readonly=true },
         new ConfigurationKey{Key="WebSocketPingInterval",Value="30",Readonly=true },
         new ConfigurationKey{Key="MeterValueSampleInterval",Value="60",Readonly=true },
         new ConfigurationKey{ Key="ClockAlignedDataInterval",Value="0",Readonly=true},
         new ConfigurationKey{ Key="MeterValuesSampledData", Value="Current.Import,Energy.Active.Import.Register,Power.Active.Import,SoC,Temperature,Voltage",Readonly=true },
        };
        if (request != null && request.Key != null && request.Key.Count > 0)
            response.ConfigurationKey = list.Where(o => request.Key.Contains(o.Key)).ToList();
        else
            response.ConfigurationKey = list;
        msgOut.JsonPayload = JsonConvert.SerializeObject(response);
        return errorCode;
    }

    private string? HandleSetChargingProfile(OCPPMessage msgIn, OCPPMessage msgOut)
    {
        // ha的ocpp设置 json[2,"5eaa10f3-f4fe-4b6b-8369-b822ebac5d3a","SetChargingProfile",
        //{ "connectorId":0,"csChargingProfiles":{ "chargingProfileId":8,"stackLevel":20,"chargingProfileKind":"Relative","chargingProfilePurpose":"ChargePointMaxProfile","chargingSchedule":{ "chargingRateUnit":"A","chargingSchedulePeriod":[{ "startPeriod":0,"limit":11.0}]} } }]

        //{"connectorId":0,"csChargingProfiles":{"chargingProfileId":8,"stackLevel":20,"chargingProfileKind":"Relative","chargingProfilePurpose":"ChargePointMaxProfile","chargingSchedule":{"chargingRateUnit":"A","chargingSchedulePeriod":[{"startPeriod":0,"limit":9.0}]}}}
        string errorCode = null;
        var request = JsonConvert.DeserializeObject<SetChargingProfileRequest>(msgIn.JsonPayload);
        var limit = request.CsChargingProfiles.ChargingSchedule.ChargingSchedulePeriod.FirstOrDefault()?.Limit;
        var unit = request.CsChargingProfiles.ChargingSchedule.ChargingRateUnit;
        if (unit == ChargingScheduleChargingRateUnit.W)
        {
            Charger.LimitPower = (decimal?)limit;
        }
        else
        {
            Charger.LimitPower = (decimal?)limit * (Charger.Voltage ?? 220);
        }

        //Charger.LimitPowerExpiredTime = DateTime.Now.AddSeconds(request.CsChargingProfiles.ChargingSchedule.Duration ?? 3600 * 24);
        var response = new SetChargingProfileResponse();
        response.Status = SetChargingProfileResponseStatus.Accepted;
        msgOut.JsonPayload = JsonConvert.SerializeObject(response);

        System.Threading.Timer timer = new System.Threading.Timer(Metervalues, null, 2000, Timeout.Infinite);

        return errorCode;
    }

    #endregion

    public async Task SendMessage(OCPPMessage msg)
    {
        if (Client.State == WebSocketState.Open)
        {
            string ocppTextMessage = null;

            if (string.IsNullOrEmpty(msg.ErrorCode))
            {
                if (msg.MessageType == "2")
                {
                    // OCPP-Request
                    ocppTextMessage = string.Format("[{0},\"{1}\",\"{2}\",{3}]", msg.MessageType, msg.UniqueId, msg.Action, msg.JsonPayload);
                }
                else
                {
                    // OCPP-Response
                    ocppTextMessage = string.Format("[{0},\"{1}\",{2}]", msg.MessageType, msg.UniqueId, msg.JsonPayload);
                }
            }
            else
            {
                ocppTextMessage = string.Format("[{0},\"{1}\",\"{2}\",\"{3}\",{4}]", msg.MessageType, msg.UniqueId, msg.ErrorCode, msg.ErrorDescription, "{}");
            }

            if (string.IsNullOrEmpty(ocppTextMessage))
            {
                // invalid message
                ocppTextMessage = string.Format("[{0},\"{1}\",\"{2}\",\"{3}\",{4}]", "4", string.Empty, ErrorCodes.ProtocolError, string.Empty, "{}");
            }


            byte[] binaryMessage = Encoding.UTF8.GetBytes(ocppTextMessage);
            logger.Info("send:" + ocppTextMessage);
            await Client.SendAsync(new ArraySegment<byte>(binaryMessage, 0, binaryMessage.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public Task StopAsync()
    {
        Client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server stopped", CancellationToken.None);
        return Task.CompletedTask;
    }

    public async Task<string> SendBootNotification()
    {
        var request = new BootNotificationRequest();

        request.ChargePointSerialNumber = "Test1234";
        request.ChargePointModel = "imeter-test";
        request.ChargePointVendor = "imeter";
        request.FirmwareVersion = "0.1.0";
        string jsonResetRequest = JsonConvert.SerializeObject(request);
        OCPPMessage msgOut = new OCPPMessage();
        msgOut.MessageType = "2";
        msgOut.Action = "BootNotification";
        msgOut.UniqueId = Guid.NewGuid().ToString("N");
        msgOut.JsonPayload = jsonResetRequest;
        msgOut.TaskCompletionSource = new TaskCompletionSource<string>();

        _requestQueue.Add(msgOut.UniqueId, msgOut);

        await SendMessage(msgOut);

        string apiResult = await msgOut.TaskCompletionSource.Task;

        return apiResult;
    }

    public async Task<string> SendMeterValues()
    {
        var request = new MeterValuesRequest();
        request.ConnectorId = 1;
        request.TransactionId = Charger.TransactionId;
        request.MeterValue = new List<MeterValue>()
        {
            new MeterValue{ Timestamp=DateTimeOffset.Now,SampledValue=GetCurrentMeterData() }
        };
        string jsonResetRequest = JsonConvert.SerializeObject(request);

        OCPPMessage msgOut = new OCPPMessage();
        msgOut.MessageType = "2";
        msgOut.Action = "MeterValues";
        msgOut.UniqueId = Guid.NewGuid().ToString("N");
        msgOut.JsonPayload = jsonResetRequest;
        msgOut.TaskCompletionSource = new TaskCompletionSource<string>();

        _requestQueue.Add(msgOut.UniqueId, msgOut);

        await SendMessage(msgOut);

        string apiResult = await msgOut.TaskCompletionSource.Task;

        return apiResult;
    }

    public async Task<string> SendStatusNotification()
    {
        var request = new StatusNotificationRequest();
        if (Charger.IsCharging)
            request.Status = StatusNotificationRequestStatus.Charging;
        else
            request.Status = StatusNotificationRequestStatus.Available;
        request.ConnectorId = 1;
        request.Timestamp = DateTimeOffset.Now;
        request.ErrorCode = StatusNotificationRequestErrorCode.NoError;

        string jsonResetRequest = JsonConvert.SerializeObject(request);

        OCPPMessage msgOut = new OCPPMessage();
        msgOut.MessageType = "2";
        msgOut.Action = "StatusNotification";
        msgOut.UniqueId = Guid.NewGuid().ToString("N");
        msgOut.JsonPayload = jsonResetRequest;
        msgOut.TaskCompletionSource = new TaskCompletionSource<string>();

        _requestQueue.Add(msgOut.UniqueId, msgOut);

        await SendMessage(msgOut);

        string apiResult = await msgOut.TaskCompletionSource.Task;

        return apiResult;

    }

    public async Task<string> SendStartTransaction()
    {
        var request = new StartTransactionRequest();
        request.ConnectorId = 1;
        request.IdTag = Charger.IdTag;
        request.MeterStart = Charger.MeterStart ?? 0;
        request.Timestamp = Charger.MeterStartTime ?? DateTimeOffset.Now;

        string jsonResetRequest = JsonConvert.SerializeObject(request);

        OCPPMessage msgOut = new OCPPMessage();
        msgOut.MessageType = "2";
        msgOut.Action = "StartTransaction";
        msgOut.UniqueId = Guid.NewGuid().ToString("N");
        msgOut.JsonPayload = jsonResetRequest;
        msgOut.TaskCompletionSource = new TaskCompletionSource<string>();

        _requestQueue.Add(msgOut.UniqueId, msgOut);

        await SendMessage(msgOut);

        string apiResult = await msgOut.TaskCompletionSource.Task;

        return apiResult;

    }

    public async Task<string> SendStopTransaction()
    {
        var request = new StopTransactionRequest();

        //2023 - 06 - 30 15:50:08:311:[2,"rTcck695cbhcNFE9","StopTransaction",{ "meterStop":155276,"reason":"EVDisconnected","transactionId":1,"timestamp":"2023-06-30T07:50:08+00:00","transactionData":[{ "timestamp":"2023-06-30T07:50:08+00:00","sampledValue":[{ "context":"Transaction.Begin","measurand":"Energy.Active.Import.Register","unit":"Wh","value":"154953"},{ "context":"Transaction.Begin","measurand":"SoC","unit":"Percent","value":"10"},{ "context":"Transaction.End","measurand":"SoC","unit":"Percent","value":"12"}]}]}]
        request.Reason = StopTransactionRequestReason.EVDisconnected;
        request.TransactionId = Charger.TransactionId;
        request.IdTag = Charger.IdTag;
        request.MeterStop = (int)(Charger.Energy ?? 0);
        request.Timestamp = Charger.MeterStopTime ?? DateTimeOffset.Now;
        request.TransactionData = new List<TransactionData>() {
         new TransactionData{ Timestamp=DateTimeOffset.Now,
          SampledValue=GetCurrentMeterData()
         }
        };

        string jsonResetRequest = JsonConvert.SerializeObject(request);

        OCPPMessage msgOut = new OCPPMessage();
        msgOut.MessageType = "2";
        msgOut.Action = "StopTransaction";
        msgOut.UniqueId = Guid.NewGuid().ToString("N");
        msgOut.JsonPayload = jsonResetRequest;
        msgOut.TaskCompletionSource = new TaskCompletionSource<string>();

        _requestQueue.Add(msgOut.UniqueId, msgOut);

        await SendMessage(msgOut);

        string apiResult = await msgOut.TaskCompletionSource.Task;

        return apiResult;

    }

    public List<SampledValue> GetCurrentMeterData()
    {
        return new List<SampledValue>{
              new SampledValue{ Value=$"{(Charger.Energy??0).ToDecimalNum(3)}",Context= SampledValueContext.Sample_Periodic,Measurand= SampledValueMeasurand.Energy_Active_Import_Register,Location= SampledValueLocation.Outlet,Unit= SampledValueUnit.Wh },
                new SampledValue{ Value=$"{(Charger.Voltage??220).ToDecimalNum(0)}",Context= SampledValueContext.Sample_Periodic,Measurand= SampledValueMeasurand.Voltage,Location= SampledValueLocation.Outlet,Unit= SampledValueUnit.V },
                new SampledValue{ Value=$"{(Charger.Current??0).ToDecimalNum(1)}",Context= SampledValueContext.Sample_Periodic,Measurand= SampledValueMeasurand.Current_Import,Location= SampledValueLocation.Outlet,Unit= SampledValueUnit.A },
                new SampledValue{ Value=$"{(Charger.Power??0).ToDecimalNum(0)}",Context= SampledValueContext.Sample_Periodic,Measurand= SampledValueMeasurand.Power_Active_Import,Location= SampledValueLocation.Outlet,Unit= SampledValueUnit.W },
                new SampledValue{ Value=$"{(Charger.SOC??0).ToDecimalNum(2)}",Context= SampledValueContext.Sample_Periodic,Measurand= SampledValueMeasurand.SoC,Location= SampledValueLocation.Outlet,Unit= SampledValueUnit.Percent },
          };
    }

}

