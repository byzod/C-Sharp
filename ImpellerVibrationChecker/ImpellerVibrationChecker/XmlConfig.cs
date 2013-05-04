/* Copyright (C) 2012  Byzod
 * Contact me : byzzod@gmail.com
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByzodToolkit
{
	/// <summary>
	/// Handle XML file to operate config or settings
	/// </summary>
	/// 以Linq to XML为基础的配置操作类
	/// 
	/// 公有构造函数：
	///		XmlConfig(string path = "Settings.xml") +2重载
	///			path可选（xml文件的保存路径，默认"Settings.xml"）
	///			不试用，使用根元素名或者使用一个XElement初始化
	///	
	/// 公有属性：
	///		OperateConfigFileResult
	///			文件操作的结果
	///	
	///		Path
	///			xml文件的路径
	///	
	///		ConfigXML
	///			xml对象，不保存的时候在内存中
	///			可以像普通XElement一样对其进行各种操作，弥补本类的不足	///		
	///	
	/// 公有文件操作方法：
	///		LoadFromFile() +1重载
	///			从文件读取xml对象到ConfigXML
	///	
	///		SaveToFile() +1重载
	///			保存ConfigXML到文件
	///	
	/// 公有工具方法：
	///		GetVal(XName node, XName subNode)
	///			读取node/subnode的值，不存在则返回null
	/// 
	///		GetAttr(XName node, XName attr) +1重载
	///			读取node或者node/subnode的属性attr的值，不存在则返回null
	/// 
	///		SetVal(XName node, XName subNode, string value)
	///			设置node/subnode的值，不存在则新建并赋值
	/// 
	///		SetAttr(XName node, XName attr, string value) +1重载
	///			设置node或者node/subnode的属性attr的值，不存在则新建并赋值
	///	
	///		Merge(XElement other, bool isOverwrite = true)
	///			将一个XElement对象other合并到ConfigXML，自动检测是否为根元素
	///			如果isOverwrite为true，会覆盖ConfigXML存在的值或属性
	///	
	///		Subtract(XElement other)
	///			从ConfiXML中减去一个XElement对象，自动检测是否为根元素
	///			减去的意思是，如果other中一个元素没有值没有属性也没有子元素，移除ConfigXML中对应的节点
	///			如果other中一个元素有属性或者子元素其中之一，则移除ConfigXML中对应的属性或者子元素，而不是这个元素本身
	///			注意：减去一个"<Name>任意值</Name>"xml对象不进行任何操作，因为Subtract方法无法移除value，请用SetVal或者直接操作代替
	///			这是因为所有节点的默认的value的值均为空字符串，当减去或者合并一个值为空的节点时，无法确定行为
	///	
	///		Distinct()
	///			例遍整个ConfigXML，清除所有Name重复的元素，只保留文档顺序的第一个
	///	
	///	私有支持方法：
	///		部分工具方法的具体实现，一些不重要的东西=v=
	///		
	///	
	public partial class XmlConfig
	{
		#region construction
		/// <summary>
		/// Initialization with given path
		/// </summary>
		/// <param name="path">The file path</param>
		public XmlConfig(string path = "Settings.xml")
		{
			ConfigXML = new XElement("Settings");
			Path = path;
		}
		/// <summary>
		/// Initialization with given root node name and path
		/// </summary>
		/// <param name="rootNodeName">The name of root node, such as 'Settings'</param>
		/// <param name="path">The file path</param>
		public XmlConfig(XName rootNodeName, string path = "Settings.xml")
		{
			ConfigXML = new XElement(rootNodeName);
			Path = path;
		}
		/// <summary>
		/// Initialization with existed XElement and given path
		/// </summary>
		/// <param name="otherXElement">Any XElement</param>
		/// <param name="path">The file path</param>
		public XmlConfig(XElement otherXElement, string path = "Settings.xml")
		{
			ConfigXML = new XElement(otherXElement);
			Path = path;
		}
		#endregion construction

		#region properties
		private const int DEFAULT_TRUSH_SIZE = 100;
		private	XElement xml;
		private List<XElement> trush = new List<XElement>(DEFAULT_TRUSH_SIZE);
		public enum OperateConfigFileResult { Succeed, FileNotExist, IOError, InvalidPath };
		/// <summary>
		/// XML file path, relative or absolute
		/// </summary>
		public string Path { get; set; }
		/// <summary>
		/// XML object
		/// </summary>
		public XElement ConfigXML
		{
			get { return xml; }
			set { xml = value; }
		}
		#endregion properties

		#region Methods
		#region fileOps
		/// <summary>
		/// Load from Path
		/// </summary>
		/// <returns>Load result</returns>
		public OperateConfigFileResult LoadFromFile()
		{
			if (System.IO.File.Exists(Path) == true)
			{
				try
				{
					xml = XElement.Load(Path);
				}
				catch (System.ArgumentNullException) { return OperateConfigFileResult.InvalidPath; }
				catch (System.ArgumentException) { return OperateConfigFileResult.InvalidPath; }
				catch { return OperateConfigFileResult.IOError; }
				return OperateConfigFileResult.Succeed;
			}
			else
			{
				return OperateConfigFileResult.FileNotExist;
			}
		}
		/// <summary>
		/// Load from given path, relative or absolute
		/// </summary>
		/// <param name="path">The uri to load</param>
		/// <returns>Load result</returns>
		public OperateConfigFileResult LoadFromFile(string path)
		{
			path = path.Length > 0 ? path : "Settings.xml";
			if (System.IO.File.Exists(path) == true)
			{
				try
				{
					xml = XElement.Load(path);
				}
				catch (System.ArgumentNullException) { return OperateConfigFileResult.InvalidPath; }
				catch (System.ArgumentException) { return OperateConfigFileResult.InvalidPath; }
				catch { return OperateConfigFileResult.IOError; }
				return OperateConfigFileResult.Succeed;
			}
			else
			{
				return OperateConfigFileResult.FileNotExist;
			}
		}

		/// <summary>
		/// Save to Path
		/// </summary>
		/// <param name="path">The uri to save</param>
		/// <returns>Save result</returns>
		public OperateConfigFileResult SaveToFile()
		{
			try
			{
				xml.Save(Path);
			}
			catch (System.ArgumentNullException) { return OperateConfigFileResult.InvalidPath; }
			catch (System.ArgumentException) { return OperateConfigFileResult.InvalidPath; }
			catch { return OperateConfigFileResult.IOError; }
			return OperateConfigFileResult.Succeed;
		}
		/// <summary>
		/// Save to given path, relative or absolute
		/// </summary>
		/// <param name="path">The uri to save</param>
		/// <returns>Save result</returns>
		public OperateConfigFileResult SaveToFile(string path)
		{
			path = path.Length > 0 ? path : "Settings.xml";
			try
			{
				xml.Save(path);
			}
			catch (System.ArgumentNullException) { return OperateConfigFileResult.InvalidPath; }
			catch (System.ArgumentException) { return OperateConfigFileResult.InvalidPath; }
			catch { return OperateConfigFileResult.IOError; }
			return OperateConfigFileResult.Succeed;
		}
		#endregion fileOps

		#region toolsMethods
		/// <summary>
		/// Get value of node
		/// </summary>
		/// <param name="node">Node name</param>
		/// <param name="subNode">SubNode name</param>
		/// <returns>Subnode value, returns null if not exists</returns>
		public string GetVal(XName node, XName subNode)
		{
			var target = xml.Element(node);
			if (target != null
				&& target.HasElements
				&& target.Element(subNode) != null)
			{
				return target.Element(subNode).Value;
			}
			else return null;
		}
		/// <summary>
		/// Get attribute value of node
		/// </summary>
		/// <param name="node">Node name</param>
		/// <param name="attr">Attribute name</param>
		/// <returns>Attribute value, returns null if not exists</returns>
		public string GetAttr(XName node, XName attr)
		{
			var target = xml.Element(node);
			if (target != null
				&& target.HasAttributes
				&& target.Attribute(attr) != null)
			{
				return target.Attribute(attr).Value;
			}
			else return null;
		}
		/// <summary>
		/// Get attribute value of node
		/// </summary>
		/// <param name="node">Node name</param>
		/// <param name="subNode">SubNode name</param>
		/// <param name="attr">Attribute name</param>
		/// <returns>Attribute value, returns null if not exists</returns>
		public string GetAttr(XName node, XName subNode, XName attr)
		{
			var target = xml.Element(node);
			if (target != null
				&& target.HasElements
				&& target.Element(subNode) != null
				&& target.Element(subNode).HasAttributes
				&& target.Element(subNode).Attribute(attr) != null)
			{
				return target.Element(subNode).Attribute(attr).Value;
			}
			return null;
		}

		
		/// <summary>
		/// Set value of node
		/// </summary>
		/// <param name="node">Node name</param>
		/// <param name="subNode">SubNode name</param>
		/// <param name="value">Subnode value to set, if is null, delete the attribute.
		/// If the node do not exists, create it then set the value</param>
		public void SetVal(XName node, XName subNode, string value)
		{	
			var target = xml.Element(node);
			if (target != null)
			{
				if (target.Element(subNode) != null)
				{
					if (value != null)
					{
						target.Element(subNode).Value = value;
					}
					else
					{
						target.Element(subNode).Remove();
					}
				}
				else
				{
					if (value != null)
					{
						target.Add(
							new XElement(subNode, value)
						);
					}
				}
			}
			else
			{
				if (value != null)
				{
					xml.Add(
						new XElement(node,
							new XElement(subNode, value)
						)
					);
				}
			}
		}
		/// <summary>
		/// Set attribute value of node
		/// </summary>
		/// <param name="node">Node name</param>
		/// <param name="attr">Attribute name</param>
		/// <param name="value">Attribute value to set, if is null, delete the attribute.
		/// If the attribute do not exists, create it then set the value</param>
		public void SetAttr(XName node, XName attr, string value)
		{
			var target = xml.Element(node);
			if (target != null)
			{
				target.SetAttributeValue(attr, value);
			}
			else
			{
				if (value != null)
				{
					xml.Add(
						new XElement(node,
							new XAttribute(attr, value)
						)
					);
				}
			}
		}
		/// <summary>
		/// Set attribute value of node
		/// </summary>
		/// <param name="node">Node name</param>
		/// <param name="subNode">SubNode name</param>
		/// <param name="attr">Attribute name</param>
		/// <param name="value">Attribute value to set, if is null, delete the attribute</param>
		public void SetAttr(XName node, XName subNode, XName attr, string value)
		{
			var target = xml.Element(node);
			if (target != null)
			{			
				if (target.Element(subNode) != null)
				{
					target.Element(subNode).SetAttributeValue(attr,value);
				}
				else
				{
					if (value != null)
					{
						target.Add(
							new XElement(subNode,
								new XAttribute(attr, value)
							)
						);
					}
				}
			}
			else
			{
				if (value != null)
				{
					xml.Add(
						new XElement(node,
							new XElement(subNode,
								new XAttribute(attr, value)
							)
						)
					);
				}
			}
		}

		/// <summary>
		/// Merge another XElement to ConfigXML
		/// <para>Say, you can't change the root element('s name) via this method</para>
		/// <para>If a attribute is null, it won't change the attribute in ConfigXML if isOverwrite is false</para>
		/// <para>However if the isOverwrite is true, it will delete the attribute</para>
		/// </summary>
		/// <param name="other">The XElement merging into us</param>
		/// <param name="isOverwrite">Should other XElement overwrite the existed elements or attributes</param>
		public void Merge(XElement other, bool isOverwrite = true)
		{
			if (other.Name == ConfigXML.Name)
			{
				MergeElement(ConfigXML, other, isOverwrite);
			}
			else
			{
				MergeElement(ConfigXML, new XElement(ConfigXML.Name, other), isOverwrite);
			}
		}
		/// <summary>
		/// Subtract another XElement from ConfigXML
		/// </summary>
		/// <param name="other">The XElement to subtract, elements that we do not have will be ignored</param>
		public void Subtract(XElement other)
		{
			if (other.Name == ConfigXML.Name)
			{				
				SubtractElement(ConfigXML, other);
			}
			else
			{
				SubtractElement(ConfigXML, new XElement(ConfigXML.Name, other));
			}
		}

		/// <summary>
		/// Remove all repeated elements, only kept the first unique one.
		/// </summary>
		public void Distinct()
		{
			RemoveRepeatElements(ConfigXML);
			foreach (var repeatedElement in trush)
			{
				repeatedElement.Remove();
			}
			trush.Clear();
		}
		#endregion toolsMethods

		#region supportMethods
		/// <summary>
		/// Merge two element
		/// </summary>
		/// <param name="mainElement">Main element</param>
		/// <param name="otherElement">Element to merge</param>
		/// <param name="isOverwrite">Should el overwrite exist element in main element</param>
		private void MergeElement(XElement mainElement, XElement otherElement, bool isOverwrite)
		{
			//合并属性
			if (otherElement.HasAttributes)
			{
				foreach (var attr in otherElement.Attributes())
				{
					if (isOverwrite
						|| !mainElement.HasAttributes
						|| mainElement.Attribute(attr.Name) == null)
					{
						mainElement.SetAttributeValue(attr.Name, attr.Value);
					}
				}
			}

			//递归合并子元素
			if (otherElement.HasElements)
			{
				foreach (var el in otherElement.Elements())
				{
					if (!mainElement.HasElements
						|| mainElement.Element(el.Name) == null)
					{
						mainElement.Add(el);
					}
					else
					{
						MergeElement(mainElement.Element(el.Name), el, isOverwrite);
					}
				}
			}
			else
			{
				//没有子元素时，合并值（value包含子元素，如果mainElement有子元素的话会被覆盖）
				//在有value的情况下还带子元素会产生很多问题，所以有子元素就不要设置value
				if (isOverwrite && otherElement.Value != "")
				{
					mainElement.Value = otherElement.Value;
				}
			}
		}

		/// <summary>
		/// Subtract element
		/// </summary>
		/// <param name="mainElement">Main element, the minuend</param>
		/// <param name="otherElement">Other element, the subtrahend</param>
		private void SubtractElement(XElement mainElement, XElement otherElement)
		{
			//减属性
			if (mainElement.HasAttributes && otherElement.HasAttributes)
			{
				var attrToDelete =
					from attrMain in mainElement.Attributes()
					join attrOther in otherElement.Attributes()
						on attrMain.Name equals attrOther.Name
					select attrMain;
				foreach (var attrMain in attrToDelete)
				{
					attrMain.Remove();
				}
			}

			//递归减子元素
			if (mainElement.HasElements && otherElement.HasElements)
			{
				var eleToSubtract =
					from eleMain in mainElement.Elements()
					join eleOther in otherElement.Elements()
						on eleMain.Name equals eleOther.Name
					select new { eleMain, eleOther };
				foreach (var ele in eleToSubtract)
				{
					//如果other无子元素，无属性且无值，则移除对应元素
					if (!ele.eleOther.HasAttributes
						&& !ele.eleOther.HasElements
						&& (ele.eleOther.Value == "" || ele.eleOther.Value == null))
					{
						ele.eleMain.Remove();
					}					
					else //否则递归
					{
						SubtractElement(ele.eleMain, ele.eleOther);
					}
				}
			}
		}

		/// <summary>
		/// Record all repeated child elements to trush, only kept the first one.
		/// <para>After the record job, call self to each child element, until there's no child anymore</para>
		/// </summary>
		/// 私有递归重复检测，将传入el元素最后的重复子元素们记录到trush，然后对每一个子元素调用自身，直到没有子元素
		/// 因为直接移除会造成foreach访问中断
		private void RemoveRepeatElements(XElement element)
		{
			var names = new List<XName>();
			foreach (var el in element.Elements())
			{
				if (names.IndexOf(el.Name) == -1)
				{
					names.Add(el.Name);
					if (el.HasElements)
					{
						RemoveRepeatElements(el);
					}
				}
				else
				{
					trush.Add(el);
				}
			}
		}
		#endregion supportMethods
		#endregion Methods
	}
}