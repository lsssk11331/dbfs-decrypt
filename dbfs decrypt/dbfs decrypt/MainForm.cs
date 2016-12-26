/*
 * Created by SharpDevelop.
 * User: msi
 * Date: 12/26/2016
 * Time: 8:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace dbfs_decrypt
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			using (var ofDlg = new OpenFileDialog()){
				textBox1.Text = "";
				ofDlg.Multiselect = true;
				if (ofDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK){
					string[] result = ofDlg.FileNames;
					int strlen = result.Length;
					int count = 0;
					foreach(string y in result){
						count++;
						if(count == strlen){
							textBox1.Text += y;
						}
						else{
							textBox1.Text += y + Environment.NewLine;
						}
					}
				}
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			foreach(string line in textBox1.Lines){
				string filename = line.Remove(line.Length - 1);
				
				FileStream fileStream = File.Open(line, FileMode.Open, FileAccess.Read);
				byte[] numArray = new byte[0];
				BinaryReader binaryReader = new BinaryReader((Stream) fileStream);
				binaryReader.BaseStream.Seek(0L, SeekOrigin.Begin);
				File.WriteAllText(filename, DecodeBytes(binaryReader.ReadBytes((int) binaryReader.BaseStream.Length)));
			}
		}
		
		public static string DecodeBytes(byte[] bytes){
			MemoryStream memoryStream = new MemoryStream(bytes);
		    byte[] rgbKey = new byte[32]{ (byte) 24, (byte) 55, (byte) 102, (byte) 24, (byte) 98, (byte) 26, (byte) 67, (byte) 29, (byte) 84, (byte) 19, (byte) 37, (byte) 118, (byte) 104, (byte) 85, (byte) 121, (byte) 27, (byte) 93, (byte) 86, (byte) 24, (byte) 55, (byte) 102, (byte) 24, (byte) 98, (byte) 26, (byte) 67, (byte) 29, (byte) 9, (byte) 2, (byte) 49, (byte) 69, (byte) 73, (byte) 92 };
		    byte[] rgbIV = new byte[16]{ (byte) 22, (byte) 56, (byte) 82, (byte) 77, (byte) 84, (byte) 31, (byte) 74, (byte) 24, (byte) 55, (byte) 102, (byte) 24, (byte) 98, (byte) 26, (byte) 67, (byte) 29, (byte) 99 };
		    RijndaelManaged rijndaelManaged = new RijndaelManaged();
		    string newline = new StreamReader((Stream) new CryptoStream((Stream) memoryStream, rijndaelManaged.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Read)).ReadToEnd();
		    newline = System.Text.RegularExpressions.Regex.Unescape(newline);
		    return newline;
		}
	}
}
