# CoffeeScript
engine.on('SetContextInfoVisibility', (visible) ->
    if (visible)
        $("#context-info").show()
    else
        $("#context-info").hide()
)