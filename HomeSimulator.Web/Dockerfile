# 使用官方的 ASP.NET Core 7.0 运行时镜像作为基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080

# 复制发布的文件到容器的工作目录中
COPY . .

# 设置容器启动时的入口点
ENTRYPOINT ["dotnet", "HomeSimulator.Web.dll"]