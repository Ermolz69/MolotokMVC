namespace MolotokMvc.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Vacancy> Vacancies { get; set; }
        public List<Resume> Resumes { get; set; }
        public int SelectRow { get; set; }
    }
}
