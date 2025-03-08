namespace ThaiYVien.Models.ViewModel
{
	public class ServiceDetailViewModel
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Duration { get; set; } 
		public decimal Price { get; set; }
		public string Description { get; set; }
		public string ImgUrl { get; set; }
        public List<TreatmentProcessesModel> TreatmentProcesses { get; set; }
	}
}
