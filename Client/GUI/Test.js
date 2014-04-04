engine.on('SetContextInfoVisibility', function (visible) {
    if (visible)
        $("#context-info").show();
    else
        $("#context-info").hide();
});