engine.on 'contextinfo.visible', (visible) ->
	$("#context-info").toggle visible
	
engine.on 'contextinfo.usealt', (use) ->
	$("#context-info #norm").toggle use
	$("#context-info #alt").toggle !use