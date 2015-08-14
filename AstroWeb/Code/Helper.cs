using System;
using AstroBuildersModel;

// PCLCrypto : http://www.c-sharpcorner.com/UploadFile/4088a7/using-cryptography-in-portable-xamarin-formswindows-phone/ 
using PCLCrypto;
using System.Text;
using System.IO;

namespace AstroWeb
{
	public static class Helper
	{
		public static CountryManager AllCountry = new CountryManager ();
		public static NewsManager AllNews = new NewsManager ();
		public static BuildersManager AllBuilders = new BuildersManager();
		public static ClubsManager AllClubs = new ClubsManager();
		public static UsersManager AllUsers = new UsersManager();
		public static ExhibitionsManager AllExhibitions = new ExhibitionsManager();

		public static void DoInit() {
			CreateFolder (AllCountry.FolderName);
			CreateFolder (AllNews.FolderName);
			CreateFolder (AllBuilders.FolderName);
			CreateFolder (AllClubs.FolderName);
			CreateFolder (AllUsers.FolderName);
			CreateFolder (AllExhibitions.FolderName);

			AllCountry.SaveFile += SaveFile;
			AllNews.SaveFile += SaveFile;
			AllBuilders.SaveFile += SaveFile;
			AllClubs.SaveFile += SaveFile;
			AllUsers.SaveFile += SaveFile;
			AllExhibitions.SaveFile += SaveFile;

			ReloadData ();

			/*
			string folder = AllCountry.FolderName;
			if (Tools.IsDirectoryEmpty (folder)) {
				foreach (Country o in AllCountry.All)
					SaveFile (folder, o.Id.ToString (), o.JSonData);
				folder = AllNews.FolderName;
				foreach (News o in AllNews.All)
					SaveFile (folder, o.Id.ToString (), o.JSonData);
				folder = AllBuilders.FolderName;
				foreach (Builder o in AllBuilders.All)
					SaveFile (folder, o.Id.ToString (), o.JSonData);
				folder = AllClubs.FolderName;
				foreach (Club o in AllClubs.All)
					SaveFile (folder, o.Id.ToString (), o.JSonData);
				folder = AllUsers.FolderName;
				foreach (User o in AllUsers.All)
					SaveFile (folder, o.Id.ToString (), o.JSonData);
			}
			*/
		}

		static void SaveFile (string folder, string name, string data)
		{
			Tools.SaveTextFile (folder, name, data);
		}

		static void CreateFolder (string folder)
		{
			Tools.CreateFolder (folder);
		}

		public static string Base64Encode(string plainText) {
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData) {
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		private static byte[] CreateSalt(uint lengthInBytes) {
			return WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
		}

		private static byte[] CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000) {
			byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);   
			return key;
		}

		private static byte[] EncryptAes(string data, string password, byte[] salt)  {
			byte[] key = CreateDerivedKey(password, salt);
			ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
			ICryptographicKey symetricKey = aes.CreateSymmetricKey(key); 
			var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));
			return bytes; 
		}

		private static string DecryptAes(byte[] data, string password, byte[] salt) {
			byte[] key = CreateDerivedKey(password, salt);
			ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
			ICryptographicKey symetricKey = aes.CreateSymmetricKey(key); 
			var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, data); 
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		public static string Encrypt(string toEncrypt) {
			var salt = CreateSalt(16);
			string saltString = Convert.ToBase64String (salt);
			var bytes = EncryptAes(toEncrypt, "Hello", salt);
			string bytesString = Convert.ToBase64String (bytes);
			return saltString + "!" + bytesString;
		}

		public static string Decrypt(string toDecrypt) {
			string[] parts = toDecrypt.Split ('!');
			if (parts.Length != 2)
				return null;
			var salt = Convert.FromBase64String (parts [0]);
			var bytes = Convert.FromBase64String (parts [1]);
			var str = DecryptAes(bytes, "Hello", salt);
			return str;
		}

		public static void ReloadData() {
			string folder = AllCountry.FolderName;
			string[] files = Tools.GetFiles (folder);
			foreach (string file in files) {
				AllCountry.AddFromJSon (Tools.LoadTextFile (file));
			}
			AllCountry.ReOrder ();

			folder = AllNews.FolderName;
			files = Tools.GetFiles (folder);
			foreach (string file in files) {
				AllNews.AddFromJSon (Tools.LoadTextFile (file));
			}
			AllNews.ReOrder ();

			folder = AllBuilders.FolderName;
			files = Tools.GetFiles (folder);
			foreach (string file in files) {
				AllBuilders.AddFromJSon (Tools.LoadTextFile (file));
			}
			AllBuilders.ReOrder ();

			folder = AllClubs.FolderName;
			files = Tools.GetFiles (folder);
			foreach (string file in files) {
				AllClubs.AddFromJSon (Tools.LoadTextFile (file));
			}
			AllClubs.ReOrder ();

			folder = AllUsers.FolderName;
			files = Tools.GetFiles (folder);
			foreach (string file in files) {
				AllUsers.AddFromJSon (Tools.LoadTextFile (file));
			}
			AllUsers.ReOrder ();

			folder = AllExhibitions.FolderName;
			files = Tools.GetFiles (folder);
			foreach (string file in files) {
				AllExhibitions.AddFromJSon (Tools.LoadTextFile (file));
			}
			AllExhibitions.ReOrder ();

			/*
			AllCountry.LoadFromJson (Tools.LoadTextFile (AllCountry.CollectionName));
			AllNews.LoadFromJson (Tools.LoadTextFile (AllNews.CollectionName));
			AllBuilders.LoadFromJson (Tools.LoadTextFile (AllBuilders.CollectionName));
			AllClubs.LoadFromJson (Tools.LoadTextFile (AllClubs.CollectionName));
			AllUsers.LoadFromJson (Tools.LoadTextFile (AllUsers.CollectionName));
			*/
		}

		public static void CreateSomeData() {
			if (AllCountry.Collection.Count == 0) {
				Country c;
				// https://countrycode.org
				c = new Country ();
				c.Title = "France";
				c.Code = "FRA";
				AllCountry.Add (c);
				c = new Country ();
				c.Title = "United States of America";
				c.Code = "USA";
				AllCountry.Add (c);
				c = new Country ();
				c.Title = "Belgium";
				c.Code = "BEL";
				AllCountry.Add (c);

				//Tools.SaveTextFile (AllCountry.CollectionName, AllCountry.Save ());
			}
			if (AllClubs.Collection.Count == 0) {
				Club c;
				c = new Club ();
				c.Title = "R2 Builders Club France";
				c.Logo = "http://r2builders.diverstrucs.com/content/Clubs/R2-FR.png";
				AllClubs.Add (c);
				c = new Club ();
				c.Title = "R2 Builders Club";
				c.Logo = "http://r2builders.diverstrucs.com/content/Clubs/R2-BLDR.png";
				AllClubs.Add (c);
				c = new Club ();
				c.Title = "R2 Builders Club United Kingdom";
				c.Logo = "http://r2builders.diverstrucs.com/content/Clubs/R2-UK.png";
				AllClubs.Add (c);
				c = new Club ();
				c.Title = "R2 Builders Club Atlanta";
				c.Logo = "http://r2builders.diverstrucs.com/content/Clubs/R2-ATL.png";
				AllClubs.Add (c);

				//Tools.SaveTextFile (AllClubs.CollectionName, AllClubs.Save ());
			}
			if (AllBuilders.Collection.Count == 0) {
				Builder b;
				b = new Builder ();
				b.IdClub = new Guid ("c8084bce-6bb9-4644-9a14-35971cd99df0");
				b.Title = "Stéphane Fardoux";
				b.NickName = "Boris";
				b.IdForum = "2634";
				b.Logo = "http://www.r2builders.fr/forum/download/file.php?avatar=2634_1366807872.gif";
				b.Email = "sfardoux@hotmail.com";
				b.Location = "Lille, France";
				b.Droids = "2 droïdes. 1 R2-D2 en cours de construction, 1 souris MSE-6 a peine commencé.";
				b.Detail = "It's me!";
				AllBuilders.Add (b);
				b = new Builder ();
				b.IdClub = new Guid ("c8084bce-6bb9-4644-9a14-35971cd99df0");
				b.Title = "Mickaël X";
				b.NickName = "Mike";
				b.IdForum = "87";
				b.Logo = "http://www.r2builders.fr/forum/download/file.php?avatar=87_1352922053.jpg";
				AllBuilders.Add (b);
				b = new Builder ();
				b.IdClub = new Guid ("c8084bce-6bb9-4644-9a14-35971cd99df0");
				b.Title = "Jean-Michel X";
				b.NickName = "SuTaiBot";
				b.IdForum = "2623";
				b.Logo = "http://www.r2builders.fr/forum/download/file.php?avatar=2623_1361701974.jpg";
				AllBuilders.Add (b);
				b = new Builder ();
				b.IdClub = new Guid ("72778ed9-c2ec-49b1-80f4-91d7f7b1e395");
				b.Title = "Laurent Devendeville";
				b.NickName = "lolo080";
				b.IdForum = "55";
				b.Email = "lolo080@r2builders.fr";
				b.Logo = "http://www.r2builders.fr/forum/download/file.php?avatar=55_1330601182.png";
				b.Location = "Amiens, France";
				b.Droids = "2 droids. (1 R2-D2 still in building stage. 1 R2-M5 complete)";
				b.Detail = "I have loved Star Wars since the beginning. I've been collecting Star Wars with busts, statues and artworks for ten years. In January, 2011, I jumped into the R2 Builders Club and started to build my first droid.";
				AllBuilders.Add (b);
				b = new Builder ();
				b.IdClub = new Guid ("c8084bce-6bb9-4644-9a14-35971cd99df0");
				b.Title = "Christophe X";
				b.NickName = "Xoff";
				b.IdForum = "69";
				b.Logo = "http://www.r2builders.fr/forum/download/file.php?avatar=69_1332196755.jpg";
				AllBuilders.Add (b);
				b = new Builder ();
				b.IdClub = new Guid ("c8084bce-6bb9-4644-9a14-35971cd99df0");
				b.Title = "Raphaël X";
				b.NickName = "raphael71";
				b.IdForum = "57";
				b.Logo = "http://www.r2builders.fr/forum/download/file.php?avatar=57_1333271536.jpg";
				AllBuilders.Add (b);
				b = new Builder ();
				b.IdClub = new Guid ("c8084bce-6bb9-4644-9a14-35971cd99df0");
				b.Title = "Cyril X";
				b.NickName = "ouessan";
				b.IdForum = "2778";
				b.Logo = "http://www.r2builders.fr/forum/download/file.php?avatar=2778_1414684021.gif";
				AllBuilders.Add (b);

				//Tools.SaveTextFile (AllBuilders.CollectionName, AllBuilders.Save ());
			}
			if (AllNews.Collection.Count == 0) {
				News news;

				news = new News ();
				news.Date = new DateTime (2015, 5, 23);
				news.Title = "Geekopolis";
				news.Detail = "C'est aujourd'hui ! Venez nous rendre visite sur le stand.";
				AllNews.Add (news);
				news = new News ();
				news.Date = new DateTime (2014, 12, 9);
				news.Title = "Geekopolis";
				news.Detail = "C'est officiel : les R2 Builders seront présent !";
				AllNews.Add (news);

				news = new News ();
				news.Date = new DateTime (2015, 9, 26);
				news.Title = "FACTS";
				news.Detail = "C'est aujourd'hui ! Venez nous rendre visite sur le stand.";
				AllNews.Add (news);
				news = new News ();
				news.Date = new DateTime (2015, 2, 5);
				news.Title = "FACTS";
				news.Detail = "C'est officiel : les R2 Builders seront présent !";
				AllNews.Add (news);

				news = new News ();
				news.Date = new DateTime (2015, 4, 9);
				news.Title = "BEAUVAIS Fête de la Science";
				news.Detail = "Le samedi 10 octobre de 10h à 17h à Beauvais, Université Jules Verne : les R2 Builders seront présent ! Venez nombreux.";
				AllNews.Add (news);

				news = new News ();
				news.Date = new DateTime (2015, 4, 18);
				news.Title = "Nancy collector ciné séries";
				news.Detail = "C'est aujourd'hui ! Venez nous rendre visite sur le stand. C'est au gymnase Bazin à Nancy.";
				AllNews.Add (news);
				news = new News ();
				news.Date = new DateTime (2015, 3, 27);
				news.Title = "Nancy collector ciné séries";
				news.Detail = "C'est les 18 et 19 avril : les R2 Builders seront présent ! Notre builder dede62 sera là pour vous accueillir et vous montrer son R2-D2. Cela se passe au gymnase Bazin à Nancy.";
				AllNews.Add (news);

				news = new News ();
				news.Date = new DateTime (2015, 2, 11);
				news.Title = "Nice Comptoir de l'imaginaire";
				news.Detail = "Le 20 mars on remet ça au café 3D le Comptoir de l'imaginaire à Nice, café restaurant impression 3D, pour un dîner fantastique. Repas à 15€, un apéro gratuit pour les costumés.";
				AllNews.Add (news);

				news = new News ();
				news.Date = new DateTime (2015, 2, 11);
				news.Title = "NICE Soirée ciné café";
				news.Detail = "Lundi 16 février à partir de 19n30, soirée ciné café spéciale Star Wars au Félix Faure à Nice.\nAu menu, discussion, quizz, petite présentation du droïde et de l'association, et on finit par le repas.\nTous les fans et cosplayers sont les bienvenus!";
				AllNews.Add (news);

				news = new News ();
				news.Date = new DateTime (2014, 11, 22);
				news.Title = "Paris Comics Expo";
				news.Detail = "C'est aujourd'hui ! Venez nous rendre visite sur le stand.";
				AllNews.Add (news);
				news = new News ();
				news.Date = new DateTime (2014, 9, 4);
				news.Title = "Paris Comics Expo";
				news.Detail = "C'est officiel : les R2 Builders seront présent !";
				AllNews.Add (news);

				news = new News ();
				news.Date = new DateTime (2014, 3, 31);
				news.Title = "EVREUX Salon du modélisme";
				news.Detail = "L'association Mini-kit27 organise le 7ème salon du modélisme d'Evreux (27). Ca se passe les 25 et 26 octobre 2014. https://minikit27.wordpress.com/notre-salon/2014-2/";
				AllNews.Add (news);
				news = new News ();

				AllNews.Refresh ();
				//Tools.SaveTextFile (AllNews.CollectionName, AllNews.Save ());
			}

			if (AllUsers.Collection.Count == 0) {
				User user;

				user = new User ();
				user.Login = "b";
				user.Password = "b";
				user.NickName = "Boris";
				user.Title = "Stéphane Fardoux";
				user.Email = "sfardoux@hotmail.com";
				user.IdBuilder = new Guid ("35190659-6da3-4ce5-b94a-6601e7a21f38");
				AllUsers.Add (user);

				user = new User ();
				user.Login = "l";
				user.Password = "l";
				user.NickName = "lolo080";
				user.Title = "Laurent Devendeville";
				user.Email = "lolo080@r2builders.fr";
				AllUsers.Add (user);

				//Tools.SaveTextFile (AllUsers.CollectionName, AllUsers.Save ());
			}

		}

	}
}