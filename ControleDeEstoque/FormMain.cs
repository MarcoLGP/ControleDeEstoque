using ControleDeEstoque.Db;
using ControleDeEstoque.Models.entities;
using MessageUtils;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        private void CbxCategoryUpdate(Context db)
        {
            cbxCategories.DataSource = db.Categories.ToList();
            cbxCategories.DisplayMember = "CategoryName";
            cbxCategories.ValueMember = "CategoryId";
        }

        private void UpdateProducts(Context db)
        {
            if (cbxCategories.Items.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
                int categoryId = (cbxCategories.SelectedItem as Category).CategoryId;
                dgvProducts.DataSource = db.Products
                    .Where(x => x.CategoryId == categoryId)
                    .Include(x => x.Category).ToList();
                dgvProducts.Columns.Remove(dgvProducts.Columns["CategoryId"]);
                dgvProducts.Columns.Remove(dgvProducts.Columns["Category"]);
                dgvProducts.Columns["clmCode"].HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dgvProducts.Columns["clmPrice"].HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                dgvProducts.Columns["clmStock"].HeaderCell.Style.Alignment =
                    DataGridViewContentAlignment.MiddleCenter;
                Cursor = Cursors.Default;
            }
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            using FormCategory form = new();
            form.Text = "Adicionar categoria...";
            if (form.ShowDialog() == DialogResult.OK)
            {
                using Context db = new();
                Category category = new()
                {
                    CategoryName = form.txtName.Text
                };
                db.Categories.Add(category);
                db.SaveChanges();
                CbxCategoryUpdate(db);
                cbxCategories.SelectedItem = category;
                SimpleMessage.Inform("Categoria adicionada com sucesso", "Informação");        
            }
        }
        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            using FormCategory form = new();
            form.Text = "Editar categoria...";
            form.txtName.Text = cbxCategories.Text;
            if (form.ShowDialog() == DialogResult.OK)
            {
                using Context db = new();
                Category category = cbxCategories.SelectedItem as Category;
                category.CategoryName = form.txtName.Text;
                db.Categories.Attach(category);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                CbxCategoryUpdate(db);
                SimpleMessage.Inform("Categoria editada com sucesso", "Informação");
            }
        }
        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            if (SimpleMessage.Confirm("Deseja realmente excluir a categoria informada", 
                "Exclusão de categoria"))
            {
                using Context db = new();
                Category category = cbxCategories.SelectedItem as Category;
                int quantityOfProducts = db.Products.Where(
                    x => x.CategoryId == category.CategoryId).Count();
                if (quantityOfProducts == 0)
                {
                    db.Attach(category);
                    db.Entry(category).State = EntityState.Deleted;
                    db.SaveChanges();
                    CbxCategoryUpdate(db);
                    SimpleMessage.Inform("Categoria removida com sucesso", "Informação");
                }
                else
                {
                    SimpleMessage.Error("Não é possível remover uma categoria com produtos.", "Erro");
                }
            }

        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            using FormProduct form = new();
            form.Text = "Adição de produto";
            form.cbxCategories.SelectedIndex = cbxCategories.SelectedIndex;
            if (form.ShowDialog() == DialogResult.OK)
            {
                using Context db = new();
                Product product = new()
                {
                    Name = form.txtProductName.Text,
                    Price = (double)form.numericProductPrice.Value,
                    Stock = (int)form.numericProductStock.Value,
                    CategoryId = (form.cbxCategories.SelectedItem as Category).CategoryId
                };
                db.Products.Add(product);
                db.SaveChanges();
                UpdateProducts(db);
                SimpleMessage.Inform("Produto adicionado com sucesso", "Informação");
            }
        }
        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            if (dgvProducts.SelectedRows.Count > 0)
            {
                selectedRow = dgvProducts.SelectedRows[0];
                Product product = selectedRow.DataBoundItem as Product;
                using FormProduct form = new();
                form.Text = "Alteração de produto...";
                form.txtProductName.Text = product.Name;
                form.numericProductPrice.Value = Convert.ToDecimal(product.Price);
                form.numericProductStock.Value = product.Stock;
                form.cbxCategories.SelectedIndex = 
                    form.cbxCategories.FindString(product.Category.CategoryName);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    using Context db = new();
                    product.Name = form.txtProductName.Text;
                    product.Stock = (int)form.numericProductStock.Value;
                    product.Price = (double)form.numericProductPrice.Value;
                    product.Category.CategoryId =
                        (form.cbxCategories.SelectedItem as Category).CategoryId;
                    db.Products.Attach(product);
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    UpdateProducts(db);
                }
            }
        }

        private void cbxCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            using Context db = new();
            UpdateProducts(db);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            using Context db = new();
            CbxCategoryUpdate(db);
            UpdateProducts(db);
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditProduct_Click(null, null);
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = null;
            if (dgvProducts.SelectedRows.Count > 0 ) 
            {
                selectedRow = dgvProducts.SelectedRows[0];
                Product product = selectedRow.DataBoundItem as Product;
                if (SimpleMessage.Confirm("Deseja realmente excluir o produto selecionado ?", 
                    "Exclusão de produto"))
                {
                    using Context db = new();
                    db.Products.Attach(product);
                    db.Entry(product).State = EntityState.Deleted;
                    db.SaveChanges();
                    UpdateProducts(db);
                    SimpleMessage.Inform("Produto excluído com sucesso", "Informação");
                }
            }
        }
    }
}