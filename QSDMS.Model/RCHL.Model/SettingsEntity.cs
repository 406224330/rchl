//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2018-07-18 15:21:41 by bruced
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
    
using System;
namespace RCHL.Model
{
    /// <summary>
    /// 数据表实体类：SettingsEntity 
    /// </summary>
    [Serializable()]
    public partial class SettingsEntity:BaseModel
    {    
		            
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string SettingId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string Name { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string Value { get; set; }

                    
		/// <summary>
		/// text:
		/// </summary>	
                 
		public string Remark { get; set; }

           
    }    
}
	