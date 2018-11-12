$().ready(function () {
    if ($.cookie('chat-closed') == '1')
        return;

    setTimeout(function() {
        //debugger;
        if ($('#jivo-chat').height() == 33) {
            $('#jivo_chat_widget').hide();
            $('#jivo-chat').show();
            $('#jivo-chat').css('height', '0px');
            $('#jivo-chat').animate({
                height: "+=" + 460
            }, 500, function() {
                if (chatTimeout) {
                    clearTimeout(chatTimeout);
                }
                chatTimeout = setTimeout(function() {
                    loadMessages(true);
                    $('.jivo-loader').remove();
                }, chatRefresh);
                loadMessages(false);

            });
        }
    }, 3500);
});