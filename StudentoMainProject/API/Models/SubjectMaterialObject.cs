using System;

namespace StudentoMainProject.API.Models
{
    public class SubjectMaterialObject
    {
#nullable enable
        public string? Id { get; set; }
#nullable disable

        public string Name { get; set; }
#nullable enable
        public string? Description { get; set; }
        public string? FileExt { get; set; }
        /// <summary>
        /// MIME Media type https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
        /// </summary>
        public string? FileType { get; set; }
        public DateTime? Added { get; set; }
        public UserObject? AddedBy { get; set; }
        public string? SubjectMaterialGroupName { set; get; }
        public UserObject? SubjectMaterialGroupAddedBy { set; get; }
        public int? SubjectTypeId { get; set; }
        public int SubjectInstanceId { get; set; }
        public int? SubjectMaterialGroupId { set; get; }
    }
}
