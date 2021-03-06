﻿using System;
using PCLCrypto;
using System.Text;

//using Refractored.Xam.Settings;
//using DeviceInfo.Plugin;
//using DeviceInfo.Plugin.Abstractions;
using Plugin.DeviceInfo;
using Plugin.Settings;
using Plugin.DeviceInfo.Abstractions;

namespace AstroBuilders
{
	public static class Helper
	{

		public static void SettingsSave<T> (string key, T value)
		{
			CrossSettings.Current.AddOrUpdateValue<T> (key, value);
		}

		public static T SettingsRead<T> (string key, T defaultValue)
		{
			return CrossSettings.Current.GetValueOrDefault<T> (key, defaultValue);
		}

		public static string GenerateAppId { get { return CrossDeviceInfo.Current.GenerateAppId (); } }

		public static IDeviceInfo DeviceInfo{ get { return CrossDeviceInfo.Current; } }

		/*
		public static string Base64Encode(string plainText) {
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData) {
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString (base64EncodedBytes, 0, base64EncodedBytes.Length);
		}
		*/

		private static byte[] CreateSalt (int lengthInBytes)
		{
			return WinRTCrypto.CryptographicBuffer.GenerateRandom (lengthInBytes);
		}


		private static byte[] CreateDerivedKey (string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000)
		{
			byte[] key = NetFxCrypto.DeriveBytes.GetBytes (password, salt, iterations, keyLengthInBytes);   
			return key;
		}

		private static byte[] EncryptAes (string data, string password, byte[] salt)
		{
			byte[] key = CreateDerivedKey (password, salt);
			ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm (SymmetricAlgorithm.AesCbcPkcs7);
			ICryptographicKey symetricKey = aes.CreateSymmetricKey (key); 
			var bytes = WinRTCrypto.CryptographicEngine.Encrypt (symetricKey, Encoding.UTF8.GetBytes (data));
			return bytes; 
		}

		private static string DecryptAes (byte[] data, string password, byte[] salt)
		{
			byte[] key = CreateDerivedKey (password, salt);
			ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm (SymmetricAlgorithm.AesCbcPkcs7);
			ICryptographicKey symetricKey = aes.CreateSymmetricKey (key); 
			var bytes = WinRTCrypto.CryptographicEngine.Decrypt (symetricKey, data); 
			return Encoding.UTF8.GetString (bytes, 0, bytes.Length);
		}

		public static string Encrypt (string toEncrypt)
		{
			var salt = CreateSalt (16);
			string saltString = Convert.ToBase64String (salt);
			var bytes = EncryptAes (toEncrypt, "Hello", salt);
			string bytesString = Convert.ToBase64String (bytes);
			return saltString + "!" + bytesString;
		}

		public static string Decrypt (string toDecrypt)
		{
			string[] parts = toDecrypt.Split ('!');
			if (parts.Length != 2)
				return null;
			var salt = Convert.FromBase64String (parts [0]);
			var bytes = Convert.FromBase64String (parts [1]);
			var str = DecryptAes (bytes, "Hello", salt);
			return str;
		}



	}
}