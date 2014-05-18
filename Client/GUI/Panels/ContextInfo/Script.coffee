engine.on 'contextinfo.visible', (visible) ->
	$ "#context-info"
		.toggle visible
	
engine.on 'contextinfo.text', (text) ->
	$ "#context-info"
		.text text