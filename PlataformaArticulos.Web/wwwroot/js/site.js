 
function wait(text = 'Espere...')
{
    $.blockUI({ message: text });
    
}
function endWait() {    
    $.unblockUI()
}
function showHide(userType = 0)
{
    if (userType == 1) {
        $('.usr_anon').hide();
        $('.usr_user,.usr_admi').show();
        
    }
    else if (userType == 2)
    {
        $('.usr_anon,.usr_admi').hide();
        $('.usr_user').show();        
    }
    else {
        $('.usr_user,.usr_admi').hide();
        $('.usr_anon').show();
        
    }

}