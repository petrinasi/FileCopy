	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Drawing;
	
	namespace ExifData
	{
		public class FileCopy
		{
			static public void Main(string[] args)
			{
				Image file = Image.FromFile(@"/Users/petri/Downloads/IMG_1415.jpg");
				if (file == null) {
					Console.WriteLine("Image not found!");
				}
				//file = Image.FromFile(args[0]);				
				/*
				int filename = 0x0320;
				
				if (file.PropertyIdList.Contains(filename)) {
					Console.WriteLine(@"Main() Filename = {0}", System.Text.Encoding.ASCII.GetString(file.GetPropertyItem(filename).Value));					
				}
				*/
				DateTime? dateTime = ExifData.DateTaken(file);
				
				if (dateTime.HasValue == true) {
					Console.WriteLine(dateTime.ToString());
				}
			}	
		}
		
		class ExifData 
		{			
			public static DateTime? DateTaken(Image file)
			{				
				int DateTakenValue = 0x9003; //36867;
				
				foreach (int id in file.PropertyIdList) {
					Console.WriteLine(id);
				}
				
				if (!file.PropertyIdList.Contains(DateTakenValue)) {
					Console.WriteLine("DateTaken(): No DateTakenValue!");
					return null;
				}
	
				string dateTakenTag = System.Text.Encoding.ASCII.GetString(file.GetPropertyItem(DateTakenValue).Value);
				Console.WriteLine("DateTaken(): {0}", dateTakenTag);
				string[] parts = dateTakenTag.Split(':', ' ');
				int year = int.Parse(parts[0]);
				int month = int.Parse(parts[1]);
				int day = int.Parse(parts[2]);
				int hour = int.Parse(parts[3]);
				int minute = int.Parse(parts[4]);
				int second = int.Parse(parts[5]);
				Console.WriteLine(dateTakenTag);
	
				return new DateTime(year, month, day, hour, minute, second);
			}			
		} 
	}
