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
    /// 数据表实体类：TeacherCommitteeEntity 
    /// </summary>
    [Serializable()]
    public partial class TeacherCommitteeEntity:BaseModel
    {    
		            
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string TeacherCommitteeId { get; set; }

                    
		/// <summary>
		/// int:
		/// </summary>	
                 
		public int? CommitLev { get; set; }

                    
		/// <summary>
		/// datetime:
		/// </summary>	
                 
		public DateTime? CommitTime { get; set; }

                    
		/// <summary>
		/// text:
		/// </summary>	
                 
		public string CommitContent { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string MemberId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string MemberName { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string ClassId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string ClassName { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string SubjectId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string SubjectName { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string StudyContent { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string StudyOrderId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string TeacherId { get; set; }

                    
		/// <summary>
		/// varchar:
		/// </summary>	
                 
		public string TeacherName { get; set; }

           
    }    
}
	