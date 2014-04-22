$.ui.dialog.prototype._focusTabbable = $.noop;

# Panels
engine.on 'AddPanel', (html) ->
	$("#panels").append html
engine.on 'RemovePanel', (pattern) ->
	$(pattern).remove()
	
# Forms
engine.on 'AddForm', (html) ->
	$("#forms").append html
engine.on 'RemoveForm', (pattern) ->
	$(pattern).remove()

engine.on 'AddScript', (src) ->
	$.getScript src
	
engine.on 'AddStyle', (src) ->
	$("<link/>"
		rel: "stylesheet",
		type: "text/css",
		href: src
	).appendTo("head");