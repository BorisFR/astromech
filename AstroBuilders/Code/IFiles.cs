using System;

namespace AstroBuilders
{

	public interface IFiles {
		bool IsExit(string name);
		string ReadFile (string name);
		byte[] ReadFileBytes (string name);
		void SaveFile (string name, string data);
		void SaveFile (string name, byte[] data);
		void DeleteFile(string name);
	}

}