using FHP.utilities;

namespace FHP.dtos.FHP.Skill
{
    public class SkillsDetailDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SkillName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
