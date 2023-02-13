using ControleDeEstoque.Db;

namespace ControleDeEstoque
{
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();
            using Context db = new();
            CbxCategoryUpdate(db);
        }

        private void CbxCategoryUpdate(Context db)
        {
            cbxCategories.DataSource = db.Categories.ToList();
            cbxCategories.DisplayMember = "CategoryName";
            cbxCategories.ValueMember = "CategoryId";
        }
    }
}
