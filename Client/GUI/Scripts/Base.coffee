engine.on 'AddHtml', (html) ->
	$("body").append html

engine.on 'AddScript', (src) ->
	$.getScript src