engine.on 'SetContextInfoVisibility', (visible) ->
	$("#context-info").toggle visible

engine.on 'AddHtml', (html) ->
	$("body").append html;