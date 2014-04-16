$("#main-menu").dialog()

engine.on 'MainMenu.Dispose', ->
	$("#main-menu").dialog "destroy"