namespace TestProject1;

public static class TestData
{
	public static string TestPres1 = """
	                                 front: "matter"
	                                 background: "#000"
	                                 foreground: "#FFF"
	                                 ###
	                                 ascii {
	                                 this is a slide!
	                                 }
	                                 ###
	                                 markdown: {
	                                 this is a slide
	                                 }
	                                 ###
	                                 youtube: "someurl.com"
	                                 background: "tomato"
	                                 ###
	                                 ascii {
	                                                                            __          __  .__               
	                                 _____________   ____   ______ ____   _____/  |______ _/  |_|__| ____   ____  
	                                 \____ \_  __ \_/ __ \ /  ___// __ \ /    \   __\__  \\   __\  |/  _ \ /    \ 
	                                 |  |_> >  | \/\  ___/ \___ \\  ___/|   |  \  |  / __ \|  | |  (  <_> )   |  \
	                                 |   __/|__|    \___  >____  >\___  >___|  /__| (____  /__| |__|\____/|___|  /
	                                 |__|               \/     \/     \/     \/          \/                    \/ 
	                                 
	                                 }
	                                 """;

	public static string TestPres2 = """
	                                 background: "#000"
	                                 foreground: "#FFF"

	                                 ###
	                                 ascii: {
	                                                                            __          __  .__               
	                                 _____________   ____   ______ ____   _____/  |______ _/  |_|__| ____   ____  
	                                 \____ \_  __ \_/ __ \ /  ___// __ \ /    \   __\__  \\   __\  |/  _ \ /    \ 
	                                 |  |_> >  | \/\  ___/ \___ \\  ___/|   |  \  |  / __ \|  | |  (  <_> )   |  \
	                                 |   __/|__|    \___  >____  >\___  >___|  /__| (____  /__| |__|\____/|___|  /
	                                 |__|               \/     \/     \/     \/          \/                    \/ 

	                                 }
	                                 ###
	                                 youtube: 'someurl.com'
	                                 background: 'tomato'
	                                 ###
	                                 ascii {
	                                 this concludes my TED talk.
	                                 }
""";
}