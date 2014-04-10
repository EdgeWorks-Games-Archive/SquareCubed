engine.on 'AddHtml', (html) ->
	$("body").append html

engine.on 'AddScript', (src) ->
	$.getScript src
	
engine.on 'AddStyle', (src) ->
	$("<link/>"
		rel: "stylesheet",
		type: "text/css",
		href: src
	).appendTo("head");