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
    /// 数据表实体类：OwnerEntity 
    /// </summary>
    [Serializable()]
    public partial class OwnerEntity:BaseModel
    {    
		            
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string OwnerId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string MemberId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string CarNumber { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string CarColor { get; set; }

                    
		/// <summary>
		/// datetime:
		/// </summary>	
                 
		public DateTime? CreateTime { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string MemberName { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string MemberMobile { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string CarFrameNum { get; set; }

                    
		/// <summary>
		/// int:
		/// </summary>	
                 
		public int? UseType { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string UseTypeName { get; set; }

                    
		/// <summary>
		/// datetime:
		/// </summary>	
                 
		public DateTime? RegisterTime { get; set; }

                    
		/// <summary>
		/// int:
		/// </summary>	
                 
		public int? CarType { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string CarTypeName { get; set; }

                    
		/// <summary>
		/// datetime:
		/// </summary>	
                 
		public DateTime? LastCheckTime { get; set; }

                    
		/// <summary>
		/// int:
		/// </summary>	
                 
		public int? PeopleCount { get; set; }

           
    }    
}
	