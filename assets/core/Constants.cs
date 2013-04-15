namespace Core {
	public struct Constants {
		public static string GAME_STUDIO = "Guy In A Hat Games";
		public static string GAME_NAME = "Traffic Flux";
		public struct Contributor {
			public Contributor(string name,string url,string handle) {
				this.name = name;
				this.handle = handle;
				this.url = url;
			}
			public string name;
			public string url;
			public string handle;
		}
		public static Contributor[] GAME_CONTRIBUTORS = {
			new Contributor("B3nGr33ni3r","http://b3ngr33ni3r.com","@b3ngr33ni3r"),
			new Contributor("Tall Matt","http://sohcahtoastudios.com","@the_tall_matt")
		};
		
	}
}

