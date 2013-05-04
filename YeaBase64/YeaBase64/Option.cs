using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace YeaBase64
{
	public delegate void UpdateEventHandler(DialogResult e);
	//for event back call
	public partial class Option : Form
	{
		private MainForm.ImageBase64Style imageBase64Style = MainForm.ImageBase64Style.None;
		private string imageBase64StyleAfter;
		private string imageBase64StyleBefor;
		private string currentLanguage;
		private bool isOptionChanged
		{
			get { return applyButton.Enabled; }
			set { applyButton.Enabled = value; }
		}

		public Option()
		{
			currentLanguage = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
			InitializeComponent();
			textBoxBase64BoxFont.ShowFont();
			textBoxTextBoxFont.ShowFont();
			LoadSettings();			
		}

		#region load settings
		/// <summary>
		/// Load settings, if load failed or file not exist, initialize instead
		/// </summary>
		private void LoadSettings()
		{		
			ConfigFile configFile = new ConfigFile();
			if (configFile.SetConfigFile(System.Text.RegularExpressions.Regex.Replace(Application.ExecutablePath, @"\..+?$", @".xml"))
				== ConfigFile.OperateConfigResult.Succeed)
			{
				//loading config cache strings
				string Base64BoxFontName = configFile.ReadConfig("General", "Base64BoxFontName");
				string Base64BoxFontSize = configFile.ReadConfig("General", "Base64BoxFontSize");
				string Base64BoxFontBold = configFile.ReadConfig("General", "Base64BoxFontBold");
				string Base64BoxFontItalic = configFile.ReadConfig("General", "Base64BoxFontItalic");
				string TextBoxFontName = configFile.ReadConfig("General", "TextBoxFontName");
				string TextBoxFontSize = configFile.ReadConfig("General", "TextBoxFontSize");
				string TextBoxFontBold = configFile.ReadConfig("General", "TextBoxFontBold");
				string TextBoxFontItalic = configFile.ReadConfig("General", "TextBoxFontItalic");
				string Language = configFile.ReadConfig("General", "Language");
				string ImageBase64StyleSet = configFile.ReadConfig("General", "ImageBase64Style");
				string ExtMonitor = configFile.ReadConfig("General", "ExtMonitor");
				string ImageBase64StyleBefore = configFile.ReadConfig("General", "ImageBase64StyleBefore");
				string ImageBase64StyleAfter = configFile.ReadConfig("General", "ImageBase64StyleAfter");
				string ImageAutoDetectEnabled = configFile.ReadConfig("General", "ImageAutoDetectEnabled");
				string AutoCopy = configFile.ReadConfig("General", "AutoCopy");
				string RealtimeConvert = configFile.ReadConfig("General", "RealtimeConvert");
				string RealtimeConvertThreshold = configFile.ReadConfig("General", "RealtimeConvertThreshold");

				//apply config
				//base64 box font setting
				//no name found
				if (Base64BoxFontName.Length == 0)				
				{
					//no name, found size area
					if (Base64BoxFontSize.Length > 0)
					{
						textBoxBase64BoxFont.Font = new Font("Georgia", 9f);
						textBoxBase64BoxFont.Tag = Convert.ToSingle(Base64BoxFontSize);
					}
					else // no size and no name
					{
						textBoxBase64BoxFont.Font = new Font("Georgia", 9F);
						textBoxBase64BoxFont.Tag = 9f;
					}
				}
				else //found name area
				{
					try
					{
						//found name, found size
						if (Base64BoxFontSize.Length > 0)
						{
							textBoxBase64BoxFont.Font = new Font(Base64BoxFontName, 9f);
							textBoxBase64BoxFont.Tag = Convert.ToSingle(Base64BoxFontSize);
						}
						else // found name, no size
						{
							textBoxBase64BoxFont.Font = new Font(Base64BoxFontName, 9F);
							textBoxBase64BoxFont.Tag = 9f;
						}
					}
					catch //invalid name, treat as no name
					{
						if (Base64BoxFontSize.Length > 0)
						{
							textBoxBase64BoxFont.Font = new Font("Georgia", 9f);
							textBoxBase64BoxFont.Tag = Convert.ToSingle(Base64BoxFontSize);
						}
						else
						{
							textBoxBase64BoxFont.Font = new Font("Georgia", 9F);
							textBoxBase64BoxFont.Tag = 9f;
						}
					}
				}
				//apply style
				if (Base64BoxFontItalic.Length > 0 && Base64BoxFontBold.Length > 0 ) // bold & italic
				{
					textBoxBase64BoxFont.Font = new Font(textBoxBase64BoxFont.Font, FontStyle.Bold | FontStyle.Italic);
				}
				else if(Base64BoxFontBold.Length > 0)// bold
				{
					textBoxBase64BoxFont.Font = new Font(textBoxBase64BoxFont.Font, FontStyle.Bold);
				}
				else if (Base64BoxFontItalic.Length > 0) //italic
				{
					textBoxBase64BoxFont.Font = new Font(textBoxBase64BoxFont.Font, FontStyle.Italic);
				}//else no changing
				textBoxBase64BoxFont.ShowFont();

				//textbox font setting
				//no name found
				if (TextBoxFontName.Length == 0)
				{
					//no name, found size area
					if (TextBoxFontSize.Length > 0)
					{
						textBoxTextBoxFont.Font = new Font("Georgia", 9f);
						textBoxTextBoxFont.Tag = Convert.ToSingle(TextBoxFontSize);
					}
					else // no size and no name
					{
						textBoxTextBoxFont.Font = new Font("Georgia", 9F);
						textBoxTextBoxFont.Tag = 9f;
					}
				}
				else //found name area
				{
					try
					{
						//found name, found size
						if (TextBoxFontSize.Length > 0)
						{
							textBoxTextBoxFont.Font = new Font(TextBoxFontName, 9f);
							textBoxTextBoxFont.Tag = Convert.ToSingle(TextBoxFontSize);
						}
						else // found name, no size
						{
							textBoxTextBoxFont.Font = new Font(TextBoxFontName, 9F);
							textBoxTextBoxFont.Tag = 9f;
						}
					}
					catch //invalid name, treat as no name
					{
						if (TextBoxFontSize.Length > 0)
						{
							textBoxTextBoxFont.Font = new Font("Georgia", 9f);
							textBoxTextBoxFont.Tag = Convert.ToSingle(TextBoxFontSize);
						}
						else
						{
							textBoxTextBoxFont.Font = new Font("Georgia", 9F);
							textBoxTextBoxFont.Tag = 9f;
						}
					}
				}
				//apply style
				if (TextBoxFontItalic.Length > 0 && TextBoxFontBold.Length > 0) // bold & italic
				{
					textBoxTextBoxFont.Font = new Font(textBoxTextBoxFont.Font, FontStyle.Bold | FontStyle.Italic);
				}
				else if (TextBoxFontBold.Length > 0)// bold
				{
					textBoxTextBoxFont.Font = new Font(textBoxTextBoxFont.Font, FontStyle.Bold);
				}
				else if (TextBoxFontItalic.Length > 0) //italic
				{
					textBoxTextBoxFont.Font = new Font(textBoxTextBoxFont.Font, FontStyle.Italic);
				}//else no changing
				textBoxTextBoxFont.ShowFont();

				//apply language
				if (Language.Length > 0)
				{
					switch (Language)
					{
						case "en-US":
							comboBoxLanguageChoice.SelectedIndex = 0;
							break;
						case "zh-CN":
							comboBoxLanguageChoice.SelectedIndex = 1;
							break;
						default:
							comboBoxLanguageChoice.Text = Language;
							break;
					}
				}
				else
				{
					switch (currentLanguage)
					{
						case "en-US":
							comboBoxLanguageChoice.SelectedIndex = 0;
							break;
						case "zh-CN":
							comboBoxLanguageChoice.SelectedIndex = 1;
							break;
						default:
							comboBoxLanguageChoice.SelectedIndex = 0;
							break;
					}
				}

				//apply image base64 style
				//DIY style string
				if (ImageBase64StyleBefore.Length > 0) imageBase64StyleBefor = ImageBase64StyleBefore;
				else imageBase64StyleBefor = "";
				if (ImageBase64StyleAfter.Length > 0) imageBase64StyleAfter = ImageBase64StyleAfter;
				else imageBase64StyleAfter = "";
				if (ExtMonitor.Length > 0) extMonitorTextBox.Text = ExtMonitor;
				else extMonitorTextBox.Text = @"apng | bmp | gif | iff | j2c | j2k | jp2 | jpc | jpe | jpeg | jpf | jpg | jpx | pcx | png | psd | ras | raw | rsb | sgi | tga | tiff | tif | wbmp";
				if (ImageAutoDetectEnabled.Length > 0) imageAutoDetectCheckBox.Checked = false;
				else imageAutoDetectCheckBox.Checked = true;

				//style chioce
				if (ImageBase64StyleSet.Length > 0)
				{
					switch (ImageBase64StyleSet)
					{
						case "None":
							radioButtonImageBase64StyleNone.Checked = true;
							break;
						case "Header":
							radioButtonImageBase64StyleHeader.Checked = true;
							break;
						case "Style":
							radioButtonImageBase64StyleStyle.Checked = true;
							break;
						case "DIY":
							radioButtonImageBase64StyleDIY.Checked = true;
							break;
						default:
							radioButtonImageBase64StyleNone.Checked = true;
							break;
					}
				}
				else
				{
					radioButtonImageBase64StyleNone.Checked = true;
				}

				//auto copy
				if (AutoCopy.Length > 0) checkBoxAutoCopy.Checked = false;
				else checkBoxAutoCopy.Checked = true;

				//realtime convert
				if (RealtimeConvert.Length > 0) checkBoxRealtimeConvert.Checked = false;
				else checkBoxRealtimeConvert.Checked = true;
				if (RealtimeConvertThreshold.Length > 0) textBoxAutoConvertThreshold.Text = RealtimeConvertThreshold;
				else textBoxAutoConvertThreshold.Text = "50000";

				//finished
				isOptionChanged = false;
			}
			else//Initialize
			{
				SetToDefault();
				isOptionChanged = false;
			}
		}
		public static string LoadLanguage()
		{			
			string CurrentLanguage = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
			ConfigFile configFile = new ConfigFile();
			if (configFile.SetConfigFile(System.Text.RegularExpressions.Regex.Replace(Application.ExecutablePath, @"\..+?$", @".xml"))
				== ConfigFile.OperateConfigResult.Succeed)
			{
				string Language = configFile.ReadConfig("General", "Language");
				//apply language
				try
				{
					if (Language.Length != 0)
					{
						System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Language);
						System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
					}
				}
				catch { return CurrentLanguage; } 
				return CurrentLanguage;
			}
			return CurrentLanguage;
		}
		#endregion

		#region buttons
		public event UpdateEventHandler UpdateButtonClick;
		private void cancelButton_Click(object sender, EventArgs e)
		{
			LoadSettings();
			this.Close();
		}
		private void saveButton_Click(object sender, EventArgs e)
		{
			ConfigFile configFile = new ConfigFile();
			//save settings
#if DEBUG
			MessageBox.Show(System.Text.RegularExpressions.Regex.Replace(Application.ExecutablePath, @"\..+?$", @".xml"));			
#endif
			if (configFile.SetConfigFile(System.Text.RegularExpressions.Regex.Replace(Application.ExecutablePath, @"\..+?$", @".xml"))
				!= ConfigFile.OperateConfigResult.IOError)
			{
				//saving configs
				if (textBoxBase64BoxFont.Font.Name != "Georgia") configFile.SaveConfig("General", "Base64BoxFontName", textBoxBase64BoxFont.Font.Name);
				else configFile.DeleteConfig("General", "Base64BoxFontName");

				if (textBoxBase64BoxFont.Tag.ToString() != "9") configFile.SaveConfig("General", "Base64BoxFontSize", textBoxBase64BoxFont.Tag.ToString());
				else configFile.DeleteConfig("General", "Base64BoxFontSize");

				if (textBoxBase64BoxFont.Font.Bold) configFile.SaveConfig("General", "Base64BoxFontBold", textBoxBase64BoxFont.Font.Bold.ToString());
				else configFile.DeleteConfig("General", "Base64BoxFontBold");

				if (textBoxBase64BoxFont.Font.Italic) configFile.SaveConfig("General", "Base64BoxFontItalic", textBoxBase64BoxFont.Font.Italic.ToString());
				else configFile.DeleteConfig("General", "Base64BoxFontItalic");

				if (textBoxTextBoxFont.Font.Name != "Georgia") configFile.SaveConfig("General", "TextBoxFontName", textBoxTextBoxFont.Font.Name);
				else configFile.DeleteConfig("General", "TextBoxFontName");

				if (textBoxTextBoxFont.Tag.ToString() != "9") configFile.SaveConfig("General", "TextBoxFontSize", textBoxTextBoxFont.Tag.ToString());
				else configFile.DeleteConfig("General", "TextBoxFontSize");

				if(textBoxTextBoxFont.Font.Bold)configFile.SaveConfig("General", "TextBoxFontBold", textBoxTextBoxFont.Font.Bold.ToString());
				else configFile.DeleteConfig("General", "TextBoxFontBold");

				if (textBoxTextBoxFont.Font.Italic) configFile.SaveConfig("General", "TextBoxFontItalic", textBoxTextBoxFont.Font.Italic.ToString());
				else configFile.DeleteConfig("General", "TextBoxFontItalic");

				if (comboBoxLanguageChoice.Text.Length != 0) configFile.SaveConfig("General", "Language", comboBoxLanguageChoice.Text);
				else configFile.ReadConfig("General", "Language");

				switch (imageBase64Style)
				{
					case MainForm.ImageBase64Style.None:
						configFile.DeleteConfig("General", "ImageBase64Style");
						break;
					case MainForm.ImageBase64Style.Header:
						configFile.SaveConfig("General", "ImageBase64Style", "Header");
						break;
					case MainForm.ImageBase64Style.Style:
						configFile.SaveConfig("General", "ImageBase64Style", "Style");
						break;
					case MainForm.ImageBase64Style.DIY:
						configFile.SaveConfig("General", "ImageBase64Style", "DIY");
						configFile.SaveConfig("General", "ImageBase64StyleBefore", textBoxStyleBefore.Text);
						configFile.SaveConfig("General", "ImageBase64StyleAfter", textBoxStyleAfter.Text);
						break;
					default:
						break;
				}

				if (extMonitorTextBox.Text != @"apng | bmp | gif | iff | j2c | j2k | jp2 | jpc | jpe | jpeg | jpf | jpg | jpx | pcx | png | psd | ras | raw | rsb | sgi | tga | tiff | tif | wbmp")
				{
					configFile.SaveConfig("General", "ExtMonitor", extMonitorTextBox.Text);
				}
				else configFile.DeleteConfig("General", "ExtMonitor");

				if (!imageAutoDetectCheckBox.Checked) configFile.SaveConfig("General", "ImageAutoDetectEnabled", imageAutoDetectCheckBox.Checked.ToString());
				else configFile.DeleteConfig("General", "ImageAutoDetectEnabled");

				if (!checkBoxAutoCopy.Checked) configFile.SaveConfig("General", "AutoCopy", checkBoxAutoCopy.Checked.ToString());
				else configFile.DeleteConfig("General", "AutoCopy");

				if (!checkBoxRealtimeConvert.Checked) configFile.SaveConfig("General", "RealtimeConvert", checkBoxRealtimeConvert.Checked.ToString());
				else configFile.DeleteConfig("General", "RealtimeConvert");

				if (textBoxAutoConvertThreshold.Text != "50000") configFile.SaveConfig("General", "RealtimeConvertThreshold", textBoxAutoConvertThreshold.Text);
				else configFile.DeleteConfig("General", "RealtimeConvertThreshold");

				configFile.SaveConfigFile();
				isOptionChanged = false;
				//update settings event
				UpdateButtonClick(DialogResult.OK);
				if(sender != applyButton) this.Close();				
			}
			else { MessageBox.Show(Properties.Resources.Option_saveFailMessage); }
		}
		private void applyButton_Click(object sender, EventArgs e)
		{
			//let the save button do the save job
			saveButton_Click(sender, e);
			//then tell the main window to fresh
			UpdateButtonClick(DialogResult.OK);
		}
		private void resetAllButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(Properties.Resources.Option_resetAllAlertText, Properties.Resources.Option_resetAllAlertTitle, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
			{
				SetToDefault();
				System.IO.File.Delete(System.Text.RegularExpressions.Regex.Replace(Application.ExecutablePath, @"\..+?$", @".xml"));
				UpdateButtonClick(DialogResult.OK);
				isOptionChanged = false;
			}
		}
		#endregion

		#region options handler
		//settings related
		//set font and show it
		private void fontChoiceBox_Click(object sender, EventArgs e)
		{
			fontDialog1.Font = new Font(((FontTextBox)sender).Font.Name, Convert.ToSingle(((FontTextBox)sender).Tag), ((FontTextBox)sender).Font.Style);
			if (fontDialog1.ShowDialog(this) == DialogResult.OK)
			{
				((FontTextBox)sender).Font = new Font(fontDialog1.Font.Name, 9F, fontDialog1.Font.Style);
				((FontTextBox)sender).Tag = fontDialog1.Font.Size;
				((FontTextBox)sender).ShowFont();
			}
		}
		private void fontChoiceBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Modifiers == Keys.None && e.KeyCode == Keys.Enter)
			{
				fontChoiceBox_Click(sender, (EventArgs)e);
			}
		}
		//image base64 style
		private void radioButtonsImageStyle_CheckedChanged(object sender, EventArgs e)
		{
			Option_Changed(sender, e);
			if (radioButtonImageBase64StyleDIY.Checked)
			{
				textBoxStyleBefore.ReadOnly = false;
				textBoxStyleAfter.ReadOnly = false;
				textBoxStyleBefore.Text = imageBase64StyleBefor;
				textBoxStyleAfter.Text = imageBase64StyleAfter;
				imageBase64Style = MainForm.ImageBase64Style.DIY;
			}
			else
			{
				textBoxStyleBefore.ReadOnly = true;
				textBoxStyleAfter.ReadOnly = true;
				if (radioButtonImageBase64StyleNone.Checked)
				{
					textBoxStyleBefore.Text = "";
					textBoxStyleAfter.Text = "";
					imageBase64Style = MainForm.ImageBase64Style.None;
				}
				if (radioButtonImageBase64StyleHeader.Checked)
				{
					textBoxStyleBefore.Text = @"data:image/png;base64,";
					textBoxStyleAfter.Text = "";
					imageBase64Style = MainForm.ImageBase64Style.Header;
				}
				if (radioButtonImageBase64StyleStyle.Checked)
				{
					textBoxStyleBefore.Text = @"<img src=""data:image/png;base64,";
					textBoxStyleAfter.Text = @""">";
					imageBase64Style = MainForm.ImageBase64Style.Style;
				}
			}
		}
		//valid numbers check: integer only 
		private void textBoxAutoConvertThreshold_TextChanged(object sender, EventArgs e)
		{
			Int64 threshold=50000;
			Option_Changed(sender, e);
			try
			{
				threshold = Convert.ToInt32(((TextBox)sender).Text);
				if (threshold > 200000)
				{
					if (MessageBox.Show(Properties.Resources.Option_realtimeConvertThresholdAlert, "Warning", MessageBoxButtons.YesNo)
						== DialogResult.No)
					{
						textBoxAutoConvertThreshold.Text = "50000";
					}
				}
			}
			catch { MessageBox.Show(Properties.Resources.Option_realtimeConvertThresholdInvalid); textBoxAutoConvertThreshold.Text = "50000"; return; }
		}

		//set startup position
		private void Option_Load(object sender, EventArgs e)
		{
			this.Location = new System.Drawing.Point(this.Owner.Location.X + 150, this.Owner.Location.Y + 50);
		}

		//is option changed
		private void Option_Changed(object sender, EventArgs e)
		{
			isOptionChanged = true;
		}

		//reset or initialize
		private void SetToDefault()
		{
			textBoxBase64BoxFont.Font = new Font("Georgia", 9F);
			textBoxBase64BoxFont.Tag = 9f;
			textBoxBase64BoxFont.ShowFont();
			textBoxTextBoxFont.Font = new Font("Georgia", 9F);
			textBoxTextBoxFont.Tag = 9f;
			textBoxTextBoxFont.ShowFont();
			switch (currentLanguage)
			{
				case "en-US":
					comboBoxLanguageChoice.SelectedIndex = 0;
					break;
				case "zh-CN":
					comboBoxLanguageChoice.SelectedIndex = 1;
					break;
				default:
					comboBoxLanguageChoice.SelectedIndex = 0;
					break;
			}
			imageBase64StyleBefor = "";
			imageBase64StyleAfter = "";
			extMonitorTextBox.Text = @"apng | bmp | gif | iff | j2c | j2k | jp2 | jpc | jpe | jpeg | jpf | jpg | jpx | pcx | png | psd | ras | raw | rsb | sgi | tga | tiff | tif | wbmp";
			imageAutoDetectCheckBox.Checked = true;
			radioButtonImageBase64StyleNone.Checked = true;
			checkBoxAutoCopy.Checked = true;
			checkBoxRealtimeConvert.Checked = true;
			textBoxAutoConvertThreshold.Text = "50000";
		}

		#endregion
	}

	#region child class and support
	#region LINQ mode config file operation
	public class ConfigFile
	{
		private string configFilePath = "";
		public enum OperateConfigResult { Succeed,FileNotExist,IOError }
		XElement configXml;
		/// <summary>
		/// Load XML config file, return load state
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public OperateConfigResult SetConfigFile(string filePath)
		{
			this.configFilePath = filePath;
			if ( System.IO.File.Exists(configFilePath)==true){			
				try
				{
					this.configXml = XElement.Load(configFilePath);
				}
				catch { return OperateConfigResult.IOError; }
			}
			else 
			{
				this.configXml = new XElement("Settings");
				return OperateConfigResult.FileNotExist;
			}
			return OperateConfigResult.Succeed;
		}

		/// <summary>
		/// Save XML config file, return load state
		/// </summary>
		/// <returns></returns>
		public OperateConfigResult SaveConfigFile()
		{
			try
			{
				this.configXml.Save(configFilePath);
			}
			catch { return OperateConfigResult.IOError; }
			return OperateConfigResult.Succeed;
		}


		/// <summary>
		/// Read the value of selected element, if not exist, return empty string.
		/// </summary>
		/// <param name="Element">The parent config element</param>
		/// <param name="SubElement">The child config element</param>
		/// <returns>The value string</returns>
		public string ReadConfig(string Element, string SubElement)
		{
			//Check the node exist or not, if not, create it.
			try
			{
				return this.configXml.Element(Element).Element(SubElement).Value;
			}
			catch //the parent node is not exist
			{	return "";	}
		}


		/// <summary>
		/// Save config to config xml file. File is saved after each save operation.
		/// </summary>
		/// <param name="Element">The parent config element</param>
		/// <param name="SubElement">The child config element</param>
		/// <param name="Value">The value of the config</param>
		public void SaveConfig(string Element, string SubElement, string Value)
		{
			//Check the node exist or not, if not, create it.
			if (this.configXml.Element(Element) == null)
			{
				this.configXml.Add(new XElement(Element, new XElement(SubElement, Value)));
			}
			else
			{
				if (this.configXml.Element(Element).Element(SubElement) == null)
				{
					this.configXml.Element(Element).Add(new XElement(SubElement, Value));
				}
				else
				{
					this.configXml.Element(Element).Element(SubElement).SetValue(Value);
				}
			}
		}


		public void DeleteConfig(string Element, string SubElement)
		{
			//Check the node exist or not, if not, no move.
			if (this.configXml.Element(Element) != null)
			{
				if (this.configXml.Element(Element).Element(SubElement) != null)
				{
					//delete sub element
					this.configXml.Element(Element).Element(SubElement).Remove();
				}
			}
		}

	}
	#endregion

	/// <summary>
	/// Inherit from textbox, add a method 'show font'
	/// </summary>
	public class FontTextBox : TextBox
	{

		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Option));
		/// <summary>
		/// Show the font at this.text
		/// </summary>
		public void ShowFont()
		{
			string fontWeight = resources.GetString("fontStyleBold");
			string fontStyle = resources.GetString("fontStyleItalic");
			if (this.Font.Bold == false) fontWeight = "";
			if (this.Font.Italic == false) fontStyle = "";
			this.Text = this.Font.Name + ", " + this.Tag.ToString() + fontWeight + fontStyle;
		}
	}
	#endregion
}