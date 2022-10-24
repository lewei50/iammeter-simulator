
function showToast(message,bgColorClass){
    $('#toastDiv').removeClass('bg-danger');
    $('#toastDiv').removeClass('bg-info');
    $('#toastDiv').addClass(bgColorClass);
    $('#toastDiv .toast-body').text(message);
    var toast = new bootstrap.Toast($('#toastDiv'), {animation: true,autohide: true, delay: 3000});
    toast.show();
}
function warnMessage(message){
    showToast(message,'bg-danger');
}
function successMessage(message){
    showToast(message,'bg-info');
}

function showResult(r,callback){
    if (r.successful == true) {
        successMessage(r.message);
        if (callback) callback();
    }
    else
        warnMessage(r.message);
}