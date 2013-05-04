#define BETA

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YeaBase64
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			textFilePathBox.Text = "";//initialize path box
			base64FilePathBox.Text = "";
			codePageTextBox.Text = "932";//initialize code page
			encodingchoiceBox1.SelectedIndex = 0; //choose ‘default’
			this.Text = "YeaBase64  v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if BETA
			this.Text += " Beta3";
#endif
			this.textBox.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
			this.base64Box.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
			isConverting = false;
			//initialize tooltips
			helpMessageToolTip.SetToolTip(this.base64FileModeCheckBox, "Read base64 string from text files instead of from the text area");
			helpMessageToolTip.SetToolTip(this.base642textbutton, "Convert base64 to text/file");
			helpMessageToolTip.SetToolTip(this.text2base64button, "Convert text/file to base64");
			helpMessageToolTip.SetToolTip(this.encodingchoiceBox1, "Choose the encoding for converting and reading/writing file");
			helpMessageToolTip.SetToolTip(this.codePageTextBox, "Entering the custom encoding code page");
			helpMessageToolTip.SetToolTip(this.base64FilePathBox, "The path of file that containing base64 string, for saving or reading");
			helpMessageToolTip.SetToolTip(this.textFilePathBox, "The path of any type of file, for saving or reading");
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			//load settings
			LoadSettings();
			this.ActiveControl = this.textBox;
		}

		public enum ImageBase64Style
		{
			None,
			Header,
			Style,
			DIY
		}

		#region Load settings
		private bool autoCopy;
		private bool realtimeConvert;
		private Int64 realtimeConvertThreshold;
		private ImageBase64Style imageBase64Style;
		private string extMonitor;
		private string imageBase64StyleBefore;
		private string imageBase64StyleAfter;
		private bool imageAutoDetectEnabled;
		private bool isConverting;

		/// <summary>
		/// Load settings, if load failed or file not exist, initialize instead
		/// </summary>
		public void LoadSettings()
		{
			ConfigFile configFile = new ConfigFile();
			if (configFile.SetConfigFile(System.Text.RegularExpressions.Regex.Replace(Application.ExecutablePath, @"\..+?$", @".xml"))
				== ConfigFile.OperateConfigResult.Succeed)
			{
				//settings cache
				string Base64BoxFontName = configFile.ReadConfig("General", "Base64BoxFontName");
				string Base64BoxFontSize = configFile.ReadConfig("General", "Base64BoxFontSize");
				string Base64BoxFontBold = configFile.ReadConfig("General", "Base64BoxFontBold");
				string Base64BoxFontItalic = configFile.ReadConfig("General", "Base64BoxFontItalic");
				string TextBoxFontName = configFile.ReadConfig("General", "TextBoxFontName");
				string TextBoxFontSize = configFile.ReadConfig("General", "TextBoxFontSize");
				string TextBoxFontBold = configFile.ReadConfig("General", "TextBoxFontBold");
				string TextBoxFontItalic = configFile.ReadConfig("General", "TextBoxFontItalic");
				string ImageBase64StyleSet = configFile.ReadConfig("General", "ImageBase64Style");
				string ExtMonitor = configFile.ReadConfig("General", "ExtMonitor");
				string ImageBase64StyleBefore = configFile.ReadConfig("General", "ImageBase64StyleBefore");
				string ImageBase64StyleAfter = configFile.ReadConfig("General", "ImageBase64StyleAfter");
				string ImageAutoDetectEnabled = configFile.ReadConfig("General", "ImageAutoDetectEnabled");
				string AutoCopy = configFile.ReadConfig("General", "AutoCopy");
				string RealtimeConvert = configFile.ReadConfig("General", "RealtimeConvert");
				string RealtimeConvertThreshold = configFile.ReadConfig("General", "RealtimeConvertThreshold");

				//apply settings
				//general
				//font
				if (Base64BoxFontName.Length == 0)
				{
					if (Base64BoxFontSize.Length > 0) base64Box.Font = new Font("Georgia", Convert.ToSingle(Base64BoxFontSize));
					else base64Box.Font = new Font("Georgia", 9F);
				}
				else //found name area
				{
					try
					{
						if (Base64BoxFontSize.Length > 0) base64Box.Font = new Font(Base64BoxFontName, Convert.ToSingle(Base64BoxFontSize));
						else base64Box.Font = new Font(Base64BoxFontName, 9F);
					}
					catch
					{
						if (Base64BoxFontSize.Length > 0) base64Box.Font = new Font("Georgia", Convert.ToSingle(Base64BoxFontSize));
						else base64Box.Font = new Font("Georgia", 9F);
					}
				}
				//apply style
				if (Base64BoxFontItalic.Length > 0 && Base64BoxFontBold.Length > 0) // bold & italic
				{
					base64Box.Font = new Font(base64Box.Font, FontStyle.Bold | FontStyle.Italic);
				}
				else if (Base64BoxFontBold.Length > 0)// bold
				{
					base64Box.Font = new Font(base64Box.Font, FontStyle.Bold);
				}
				else if (Base64BoxFontItalic.Length > 0) //italic
				{
					base64Box.Font = new Font(base64Box.Font, FontStyle.Italic);
				}//else no changing


				if (TextBoxFontName.Length == 0)
				{
					if (TextBoxFontSize.Length > 0) textBox.Font = new Font("Georgia", Convert.ToSingle(TextBoxFontSize));
					else textBox.Font = new Font("Georgia", 9F);
				}
				else //found name area
				{
					try
					{
						if (TextBoxFontSize.Length > 0) textBox.Font = new Font(TextBoxFontName, Convert.ToSingle(TextBoxFontSize));
						else textBox.Font = new Font(TextBoxFontName, 9F);
					}
					catch
					{
						if (TextBoxFontSize.Length > 0) textBox.Font = new Font("Georgia", Convert.ToSingle(TextBoxFontSize));
						else textBox.Font = new Font("Georgia", 9F);
					}
				}
				//apply style
				if (TextBoxFontItalic.Length > 0 && TextBoxFontBold.Length > 0) // bold & italic
				{
					textBox.Font = new Font(textBox.Font, FontStyle.Bold | FontStyle.Italic);
				}
				else if (TextBoxFontBold.Length > 0)// bold
				{
					textBox.Font = new Font(textBox.Font, FontStyle.Bold);
				}
				else if (TextBoxFontItalic.Length > 0) //italic
				{
					textBox.Font = new Font(textBox.Font, FontStyle.Italic);
				}//else no changing

				//apply image base64 style
				//DIY style string
				if (ImageBase64StyleBefore.Length > 0) imageBase64StyleBefore = ImageBase64StyleBefore;
				else imageBase64StyleBefore = "";
				if (ImageBase64StyleAfter.Length > 0) imageBase64StyleAfter = ImageBase64StyleAfter;
				else imageBase64StyleAfter = "";
				if (ExtMonitor.Length > 0) extMonitor = ExtMonitor;
				else extMonitor = @"apng | bmp | gif | iff | j2c | j2k | jp2 | jpc | jpe | jpeg | jpf | jpg | jpx | pcx | png | psd | ras | raw | rsb | sgi | tga | tiff | tif | wbmp";
				if (ImageAutoDetectEnabled.Length > 0) imageAutoDetectEnabled = false;
				else imageAutoDetectEnabled = true;

				//style chioce
				if (ImageBase64StyleSet.Length > 0)
				{
					switch (ImageBase64StyleSet)
					{
						case "None":
							imageBase64Style = ImageBase64Style.None;
							break;
						case "Header":
							imageBase64Style = ImageBase64Style.Header;
							break;
						case "Style":
							imageBase64Style = ImageBase64Style.Style;
							break;
						case "DIY":
							imageBase64Style = ImageBase64Style.DIY;
							break;
						default:
							imageBase64Style = ImageBase64Style.None;
							break;
					}
				}

				//auto copy
				if (AutoCopy.Length > 0) autoCopy = false;
				else autoCopy = true;

				//realtime convert
				if (RealtimeConvert.Length > 0) realtimeConvert = false;
				else realtimeConvert = true;
				if (RealtimeConvertThreshold.Length > 0)
				{
					try { realtimeConvertThreshold = Convert.ToInt32(RealtimeConvertThreshold); }
					catch { realtimeConvertThreshold = 50000; }
				}
				else realtimeConvertThreshold = 50000;
			}
			else
			{
				//initialize
				textBox.Font = new System.Drawing.Font("Georgia", 9F);
				base64Box.Font = new System.Drawing.Font("Georgia", 9F);
				imageBase64Style = ImageBase64Style.None;
				extMonitor = @"apng | bmp | gif | iff | j2c | j2k | jp2 | jpc | jpe | jpeg | jpf | jpg | jpx | pcx | png | psd | ras | raw | rsb | sgi | tga | tiff | tif | wbmp";
				imageBase64StyleBefore = "";
				imageBase64StyleAfter = "";
				imageAutoDetectEnabled = true;
				autoCopy = true;
				realtimeConvert = true;
				realtimeConvertThreshold = 50000;
			}
		}
		#endregion

		#region inputboxes
		//base64 to text
		private void base642textbutton_Click(object sender, EventArgs e)
		{	
			string base64Str = base64Box.Text;
			byte[] decodedDataBytes,base64Bytes = new byte[5];
			if (base64Str.Length != 0 || base64FileModeCheckBox.Checked == true)
			{
				try
				{
					//get decoded data bytes from base64 text box or file
					if (base64FileModeCheckBox.Checked == true)
					{
						try
						{
							FileStream fs = new FileStream(Regex.Replace(base64FilePathBox.Text, "(\"|\')", ""), FileMode.Open);
							base64Bytes = new byte[fs.Length];
							fs.Read(base64Bytes, 0, (int)fs.Length);
							fs.Close();
						}
						catch
						{
							MessageBox.Show(Properties.Resources.MainForm_invalidPathAlert);
							return;
						}
						try
						{
							//read from file with ASCII encoding, not support UTF-8 or other format files
							base64Str = Encoding.ASCII.GetString(base64Bytes);
							decodedDataBytes = Convert.FromBase64String(base64Str);
						}
						catch
						{
							MessageBox.Show(Properties.Resources.MainForm_invalidBase64Alert);
							return;
						}
					}
					else
					{
#if DEBUG_BASE64TOFILE
						MessageBox.Show(@"length:" + base64Str.Length.ToString());
#endif
						decodedDataBytes = Convert.FromBase64String(base64Str);
					}
					//output decoded text to text box or file
					switch (encodingchoiceBox1.SelectedIndex)
					{
						case 0: //defualt
							if (sender == base642textbutton) isConverting = true;
							textBox.Text = Encoding .Default .GetString(decodedDataBytes);
							if (sender == base642textbutton) isConverting = false;
							ASAutoCopy(labelTextCopied, textBox, sender==base642textbutton);
							break;
						case 1: //ASCII
							if (sender == base642textbutton) isConverting = true;
							textBox.Text = Encoding .ASCII .GetString(decodedDataBytes);
							if (sender == base642textbutton) isConverting = false;
							ASAutoCopy(labelTextCopied, textBox, sender==base642textbutton);
							break;
						case 2: //UTF-8
							if (sender == base642textbutton) isConverting = true;
							textBox.Text = Encoding .UTF8 .GetString(decodedDataBytes);
							if (sender == base642textbutton) isConverting = false;
							ASAutoCopy(labelTextCopied, textBox, sender==base642textbutton);
							break;
						case 3: //Unicode
							if (sender == base642textbutton) isConverting = true;
							textBox.Text = Encoding .Unicode .GetString(decodedDataBytes);
							if (sender == base642textbutton) isConverting = false;
							ASAutoCopy(labelTextCopied, textBox, sender==base642textbutton);
							break;
						case 4: //Unicode-BigEndian
							if (sender == base642textbutton) isConverting = true;
							textBox.Text = Encoding .BigEndianUnicode .GetString(decodedDataBytes);
							if (sender == base642textbutton) isConverting = false;
							ASAutoCopy(labelTextCopied, textBox, sender==base642textbutton);
							break;
						case 5: //Binary
							string toWriteFilePath = Regex.Replace(textFilePathBox.Text, "(\"|\')", "");
							try
							{
								if (File.Exists(toWriteFilePath))
								{
									if (MessageBox.Show(Properties.Resources.MainForm_fileExistedAlert, Properties.Resources.MainForm_fileExistedAlertTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
									{
										FileStream fs = new FileStream(toWriteFilePath, FileMode.Truncate, FileAccess.Write);
										fs.Write(decodedDataBytes, 0, decodedDataBytes.Length);
										fs.Close();
									}
									else
									{
										return;
									}
								}
								else
								{
									FileStream fs = new FileStream(toWriteFilePath, FileMode.CreateNew);
									fs.Write(decodedDataBytes, 0, decodedDataBytes.Length);
									fs.Close();
								}
							}
							catch
							{
								MessageBox.Show(Properties.Resources.MainForm_invalidPathAlert);
								return;
							}
							break;
						case 6: //Code page
							int codePage = 0;
							try
							{
								codePage = Convert.ToInt32(codePageTextBox.Text);
#if DEBUG_CODEPAGE
								MessageBox.Show(codePage.ToString());
#endif
								if (sender == base642textbutton) isConverting = true;
								textBox.Text = Encoding.GetEncoding(codePage).GetString(decodedDataBytes);
								if (sender == base642textbutton) isConverting = false;
								ASAutoCopy(labelTextCopied, textBox, sender == base642textbutton);
							}
							catch
							{
								CodePageHelp codePageHelpBox = new CodePageHelp();
								codePageHelpBox.OpenHelpFormHandler += new CodePageHelp.openHelpFormHandler(helpHelpToolStripMenuItem_Click);
								codePageHelpBox.ShowDialog();
								//fakeProgressBar.Visible = false; 
								return;
							}
							break;
						default: //??
							if (sender == base642textbutton) isConverting = true;
							textBox.Text = Encoding .Default .GetString(decodedDataBytes);
							if (sender == base642textbutton) isConverting = false;
							ASAutoCopy(labelTextCopied, textBox, sender == base642textbutton);
							break;
					}
				}
				catch
				{
					MessageBox.Show(Properties.Resources.MainForm_invalidBase64Alert);
					return;
				}
			}
		}


		//text to base64
		private void text2base64button_Click(object sender, EventArgs e)
		{
			string textStr="";
			if (textBox.Text.Length != 0 || encodingchoiceBox1.SelectedIndex ==5)
			{
				string filePath="";
				byte[] encodedDataBytes = new byte[5];
				//get encoded data bytes array from text box or file
				switch (encodingchoiceBox1.SelectedIndex)
				{
					case 0: //defualt
						encodedDataBytes = Encoding .Default .GetBytes(textBox.Text);
						break;
					case 1: //ASCII
						encodedDataBytes = Encoding .ASCII .GetBytes(textBox.Text);
						break;
					case 2: //UTF-8
						encodedDataBytes = Encoding .UTF8 .GetBytes(textBox.Text);
						break;
					case 3: //Unicode
						encodedDataBytes = Encoding .Unicode .GetBytes(textBox.Text); 
						break;
					case 4: //Unicode-BigEndian
						encodedDataBytes = Encoding .BigEndianUnicode .GetBytes(textBox.Text);
						break;
					case 5: //Binary
						try
						{
							FileStream fs = new FileStream(filePath = Regex.Replace(textFilePathBox.Text, "(\"|\')", ""), FileMode.Open);
							encodedDataBytes = new byte[fs.Length];
							fs.Read(encodedDataBytes, 0, (int)fs.Length);
							fs.Close();
						}
						catch
						{
							MessageBox.Show(Properties.Resources.MainForm_invalidPathAlert);
							return;
						}
						break;
					case 6: //Code page
						int codePage = 0;
						try
						{
							codePage = Convert.ToInt32(codePageTextBox.Text);
#if DEBUG_CODEPAGE
							MessageBox.Show(codePage.ToString());
#endif
							encodedDataBytes = Encoding.GetEncoding(codePage).GetBytes(textBox.Text);
						}
						catch
						{
							CodePageHelp codePageHelpBox = new CodePageHelp();
							codePageHelpBox.OpenHelpFormHandler += new CodePageHelp.openHelpFormHandler(helpHelpToolStripMenuItem_Click);
							codePageHelpBox.ShowDialog();
							return;
						}
						break;
					default: //??
						encodedDataBytes = Encoding .Default .GetBytes(textBox.Text);
						break;
				}
				//output base64 string to base64 text box or file
				if (base64FileModeCheckBox.Checked == true)
				{
					string toWriteFilePath = Regex.Replace(base64FilePathBox.Text, "(\"|\')", "");
					try
					{
						byte[] textBytes = new byte[5];
						if (File.Exists(toWriteFilePath))
						{
							if (MessageBox.Show(Properties.Resources.MainForm_fileExistedAlert, Properties.Resources.MainForm_fileExistedAlertTitle, MessageBoxButtons.YesNo) == DialogResult.Yes)
							{
								//if file exists, clear it before write
								FileStream fs = new FileStream(toWriteFilePath, FileMode.Truncate);
								textStr = Convert.ToBase64String(encodedDataBytes);
								if (filePath.Length > 0) textStr = ASAddStyle(textStr, imageBase64Style, filePath);
								textBytes = Encoding.ASCII.GetBytes(textStr);
								fs.Write(textBytes, 0, textBytes.Length);
								fs.Close();
							}
							else
							{
								//fakeProgressBar.Visible = false; 
								return;
							}
						}
						else
						{
							FileStream fs = new FileStream(toWriteFilePath, FileMode.CreateNew);
							textStr = Convert.ToBase64String(encodedDataBytes);
							//write to with ASCII encoding, not support UTF-8 or other format files
							if (filePath.Length > 0) textStr = ASAddStyle(textStr, imageBase64Style, filePath);
							textBytes = Encoding.ASCII.GetBytes(textStr);
							fs.Write(textBytes, 0, textBytes.Length);
							fs.Close();
						}
					}
					catch
					{
						MessageBox.Show(Properties.Resources.MainForm_invalidPathAlert);
						return;
					}
				}
				else
				{
					textStr = Convert.ToBase64String(encodedDataBytes);
					if (sender == text2base64button) isConverting = true;
					if (filePath.Length > 0)
					{
						textStr = ASAddStyle(textStr, imageBase64Style, filePath);
					}
					base64Box.Text = textStr;
					if (sender == text2base64button) isConverting = false;
					ASAutoCopy(labelBase64Copied, base64Box, sender == text2base64button);
				}
			}
		}
		#endregion

		#region eyecandy
		//eyecandy, dynamic UI with different encoding choice box choice and base64 file mode check box
		private void encodingchoiceBox1_SelectedValueChanged(object sender, EventArgs e)
		{
			switch (encodingchoiceBox1.SelectedIndex)
			{
				case 5:
					textBox.Visible = false;				//hide text box
					textFilePathBox.Visible = true;			//show path box in text area
					textDropPanel.Visible = true;			//show drop panel in text area
					openTextFileButton.Visible = true;		//show open file button in text area
					codePageTextBox.Visible = false;		//hide code page input box
					break;
				case 6:
					codePageTextBox.Visible = true;
					textBox.Visible = true;
					textFilePathBox.Visible = false;
					textDropPanel.Visible = false;
					openTextFileButton.Visible = false;
					break;
				default:
					textBox.Visible = true;
					textFilePathBox.Visible = false;
					textDropPanel.Visible = false;
					openTextFileButton.Visible = false;
					codePageTextBox.Visible = false;
					break;
			}
		}
		private void fileModeCheckBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (base64FileModeCheckBox.Checked == true)
			{
				base64Box.Visible = false;				//hide base64 box
				base64FilePathBox.Visible = true;		//show path box in base64 area
				base64DropPanel.Visible = true;			//show drop panel in base64 area
				openBase64FileButton.Visible = true;	//show open file button in base64 area
			}
			else
			{
				base64Box.Visible = true;
				base64FilePathBox.Visible = false;
				base64DropPanel.Visible = false;
				openBase64FileButton.Visible = false;
			}
		}
		#endregion

		#region events handler
		#region hotkey handler
		private void textBox_SelectAll(object sender, KeyEventArgs e)
		{
			//ctrl+a ==  select all
			if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
			{	
				if (sender.GetType() == typeof(RichTextBox))
				{
					((RichTextBox)sender).SelectAll();
				}
				else if (sender.GetType() == typeof(TextBox))
				{
					((TextBox)sender).SelectAll();
				}
			}			
		}
		private void otherHotkeyHandler(object sender, KeyEventArgs e)
		{
			//esc == select encoding combo box
			if (e.Modifiers == Keys.None && e.KeyCode == Keys.Escape)
			{
				encodingchoiceBox1.Focus();
				encodingchoiceBox1.Capture = true;
			}
			//F1 == open help window
			if (e.Modifiers == Keys.None && e.KeyCode == Keys.F1)
			{
				if (helpForm == null || helpForm.IsDisposed)
				{
					helpForm = new HelpForm();
					helpForm.Show();
				}
				else { helpForm.Activate(); }
			}
		}
		#endregion

		#region text or file drag drop handler
		private void Box_DragEnter(object sender, DragEventArgs e)
		{
			//only text in the mouse and file drop are accpted
			if (e.Data.GetDataPresent(DataFormats.Text) || e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else
				e.Effect = DragDropEffects.None;
		}
		private void Box_DragDrop(object sender, DragEventArgs e)
		{
			string filePath = "";
			if (e.Data.GetDataPresent(DataFormats.Text))
			{
				((RichTextBox)sender).Text = e.Data.GetData(DataFormats.Text).ToString();
			}
			else  //file drop
			{
				//show the content of file in the text/base64 box
				//or turn to binary mode for image files
				filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0].ToString();
				if (!IsImageFile_DragedDrop(sender,filePath))
				{
					switch (encodingchoiceBox1.SelectedIndex)
					{
						case 0: //defualt
							((RichTextBox)sender).Text = File.ReadAllText(filePath, Encoding.Default);
							break;
						case 1: //ASCII
							((RichTextBox)sender).Text = File.ReadAllText(filePath, Encoding.ASCII);
							break;
						case 2: //UTF-8
							((RichTextBox)sender).Text = File.ReadAllText(filePath, Encoding.UTF8);
							break;
						case 3: //Unicode
							((RichTextBox)sender).Text = File.ReadAllText(filePath, Encoding.Unicode);
							break;
						case 4: //Unicode-BigEndian
							((RichTextBox)sender).Text = File.ReadAllText(filePath, Encoding.BigEndianUnicode);
							break;
						case 6: //Code page
							int codePage = 0;
							try
							{
								codePage = Convert.ToInt32(codePageTextBox.Text);
#if DEBUG_CODEPAGE
								MessageBox.Show(codePage.ToString());
#endif
								((RichTextBox)sender).Text = File.ReadAllText(filePath, Encoding.GetEncoding(codePage));
							}
							catch
							{
								CodePageHelp codePageHelpBox = new CodePageHelp();
								codePageHelpBox.OpenHelpFormHandler += new CodePageHelp.openHelpFormHandler(helpHelpToolStripMenuItem_Click);
								codePageHelpBox.ShowDialog();
								return;
							}
							break;
						default: //??
							((RichTextBox)sender).Text = File.ReadAllText(filePath, Encoding.Default);
							break;
					}
				}
			}
		}
		private bool IsImageFile_DragedDrop(object sender, string  filepath)
		{
			string fileExt = "";
			string imageFileExt = Regex.Replace(extMonitor, @"[ 　]+", @"");
			fileExt = Regex.Replace(filepath, @".*\.([^.]+)$", @"$1");
			if (fileExt.Length > 0 
				&& Regex.IsMatch(fileExt, imageFileExt, RegexOptions.IgnoreCase)  //in monitored file ext.
				&& sender == textBox  //do not touch those from base64 box
				&& imageAutoDetectEnabled)  //function enabled
			{
				encodingchoiceBox1.SelectedIndex = 5; //binary mode
				textFilePathBox.Text = filepath;
				if(realtimeConvert) text2base64button_Click(sender, new EventArgs());
				return true; //it is image file
			}
			return false; //it's not
		}

		#endregion

		#region file drop for file path in binary/file mode
		private void droppanel_DragEnter(object sender, DragEventArgs e)
		{
			//only file drop are accpted
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Link;
			else
				e.Effect = DragDropEffects.None;
		}
		private void droppanel1_DragDrop(object sender, DragEventArgs e)
		{	
			//drop file on the drop panel in text area
			string FilePaths = "";
			FilePaths = FilePaths + ((string[])e.Data.GetData(DataFormats.FileDrop))[0].ToString();
			this.textFilePathBox.Text = FilePaths;
			FileModeRealtimeConvert(textBox);
		}
		private void droppanel2_DragDrop(object sender, DragEventArgs e)
		{
			//drop file on the drop panel in base64 area
			string FilePaths = "";
			FilePaths = FilePaths + ((string[])e.Data.GetData(DataFormats.FileDrop))[0].ToString();
			this.base64FilePathBox.Text = FilePaths;
			FileModeRealtimeConvert(base64Box);			
		}
		#endregion 

		#region menu handler
		HelpForm helpForm;
		//open/save file dialog box
		private void openFileButton1_Click(object sender, EventArgs e)
		{
			//dialog box in text area
			saveFileDialog1.Filter = "All|*.*|Text|*.txt|Image|*.jpg;*.png;*.bmp|Executable|*.com;*.bat;*.exe;*.msi";
			saveFileDialog1.ShowDialog();
			this.textFilePathBox.Text = saveFileDialog1.FileName;
		}
		private void openFileButton2_Click(object sender, EventArgs e)
		{
			//dialog box in base64 area
			saveFileDialog1.Filter = "Text|*.txt|Base64 String|*.b64|All|*.*";
			saveFileDialog1.ShowDialog();
			this.base64FilePathBox.Text = saveFileDialog1.FileName;
		}

		private void helpHelpToolStripMenuItem_Click(object sender, EventArgs e)
		{	
			if (helpForm == null|| helpForm.IsDisposed)
			{
				helpForm = new HelpForm();
				helpForm.Show(this);
			}
			else { helpForm.Activate(); }
		}
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox aboutBox = new AboutBox();
			aboutBox.ShowDialog();
		}
		private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Changelog changelog = new Changelog();
			changelog.ShowDialog();
		}
		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Option option = new Option();
			option.UpdateButtonClick +=new UpdateEventHandler(option_UpdateButtonClick);
			option.ShowDialog(this);
		}
		#endregion 

		//option update handle r	
		public void option_UpdateButtonClick(DialogResult e)
		{
			LoadSettings();
		}
		#endregion

		#region Addtionl Style series
		/// <summary>
		/// Returen string with added style
		/// </summary>
		/// <param name="rawstring">Input string</param>
		/// <param name="style">What style to add</param>
		/// <param name="filepath">Used to check if it is a image</param>
		/// <returns>The string with added style</returns>
		private string ASAddStyle(string rawstring, ImageBase64Style style, string filepath)
		{
			string fileExt = "";
			string imageFileExt = Regex.Replace(extMonitor, @"[ 　]+", @""); 
			fileExt = Regex.Replace(filepath, @".*\.([^.]+)$", @"$1");
#if DEBUG
			MessageBox.Show(fileExt + "  find:" + Regex.IsMatch(fileExt, imageFileExt, RegexOptions.IgnoreCase).ToString());
#endif
			if (fileExt.Length > 0 && Regex.IsMatch(fileExt,imageFileExt,RegexOptions.IgnoreCase))
			{
				switch (style)
				{
					case ImageBase64Style.None:
						return rawstring;
					case ImageBase64Style.Header:
						return @"data:image/png;base64," + rawstring;
					case ImageBase64Style.Style:
						return @"<img src=""data:image/png;base64," + rawstring + @""">";
					case ImageBase64Style.DIY:
						return imageBase64StyleBefore + rawstring + imageBase64StyleAfter;
					default:
						return rawstring;
				}
			}
			else return rawstring;
		}

		/// <summary>
		/// Auto copy after convert
		/// </summary>
		/// <param name="target">The target label to show minder</param>
		/// <param name="boss">The target richtextbox contains the string converted</param>
		private void ASAutoCopy(Label target, RichTextBox boss, bool tryselectall)
		{
			if (autoCopy)
			{
				if (boss.Text.Length > 0)
				{
					try
					{
						Clipboard.SetText(boss.Text);
					}catch{}
				}
				Timer timer = new Timer();
				timer.Tick += new EventHandler(copiedMessageTimer_Tick);
				int COPIEDLABELDURATION = 500;
				timer.Interval = COPIEDLABELDURATION;
				target.Visible = true;
				if (tryselectall || !realtimeConvert || base64FileModeCheckBox.Checked || encodingchoiceBox1.SelectedIndex == 5)
				{
					boss.Focus();
					boss.SelectAll();
				}
				timer.Tag = target;
				timer.Enabled = true;
			}
		}
		private void copiedMessageTimer_Tick(object sender, EventArgs e)
		{
			((Timer)sender).Enabled = false;
			((Label)((Timer)sender).Tag).Visible = false;
		}

		//AS series: realtime convert
		private void textBox_TextChanged(object sender, EventArgs e)
		{
			if (!isConverting && realtimeConvert && textBox.Text.Length <= realtimeConvertThreshold
				&& (!base64FileModeCheckBox.Checked) && encodingchoiceBox1.SelectedIndex != 5 ) //binary mode
			{
				isConverting = true;
				text2base64button_Click(sender, e);
				isConverting = false;
			}
		}
		private void base64Box_TextChanged(object sender, EventArgs e)
		{
			if (!isConverting && realtimeConvert && base64Box.Text.Length <= realtimeConvertThreshold
				&& (!base64FileModeCheckBox.Checked) && encodingchoiceBox1.SelectedIndex != 5 )//binary mode
			{
				isConverting = true;
				base642textbutton_Click(sender, e);
				isConverting = false;
			}
		}
		private void FileModeRealtimeConvert(object source)
		{
			if (source == textBox && realtimeConvert)
			{
				text2base64button_Click(source, new EventArgs());
			} 
			if (source == base64Box && realtimeConvert)
			{
				base642textbutton_Click(source, new EventArgs());
			}
		}
		#endregion
	}
}