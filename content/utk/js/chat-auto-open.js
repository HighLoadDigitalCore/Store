$().ready(function() {
    setTimeout(function() {
        
            $('#jivo_chat_widget').hide();
            $('#jivo-chat').show();

            $('#jivo-chat').animate({
                height: "+=" + 297
            }, 500, function () {
                if (chatTimeout) {
                    clearTimeout(chatTimeout);
                }
                chatTimeout = setTimeout(function () {
                    loadMessages(true);
                }, chatRefresh);
                loadMessages(false);

            });
    }, 2000);
});