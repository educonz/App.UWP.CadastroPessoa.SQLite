using App.SQLite.Helpers;
using App.SQLite.Model;
using App.SQLite.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App.SQLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly PessoaService _pessoaService;
        private int _id;

        public MainPage()
        {
            this.InitializeComponent();

            _pessoaService = new PessoaService();
            CarregarLista();
            LimparCampos();
        }

        private void CarregarLista()
        {
            lista.ItemsSource = _pessoaService.GetAll().ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(edtNome.Text))
            {
                ShowMessage("Nome não foi informado!");
                return;
            }

            if (_id > 0)
            {
                _pessoaService.Update(new Pessoa
                {
                    Id = _id,
                    Nome = edtNome.Text,
                    CPF = edtCPF.Text,
                    Email = edtEmail.Text
                });
            }
            else
            {
                _pessoaService.Insert(new Pessoa
                {
                    Nome = edtNome.Text,
                    CPF = edtCPF.Text,
                    Email = edtEmail.Text
                });
            }

            ShowMessage("Salvo com sucesso!");
            LimparCampos();
            CarregarLista();
        }

        private async void ShowMessage(string message)
        {
            await new MessageDialog(message).ShowAsync();
        }

        private async void ShowMessageQuestion(string message, Action action)
        {
            var messageDialog = new MessageDialog(message);

            var comandoSim = new UICommand("Sim");
            var comandoNao = new UICommand("Não");

            messageDialog.Commands.Add(comandoSim);
            messageDialog.Commands.Add(comandoNao);

            var comandoResultado = await messageDialog.ShowAsync();

            if (comandoResultado == comandoSim)
            {
                action.Invoke();
            }
        }

        private void LimparCampos()
        {
            edtNome.Text = string.Empty;
            edtCPF.Text = string.Empty;
            edtEmail.Text = string.Empty;
            _id = 0;
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            var pessoa = ObterPessoaSelecionada(sender);

            _id = pessoa.Id;
            edtNome.Text = pessoa.Nome;
            edtCPF.Text = pessoa.CPF;
            edtEmail.Text = pessoa.Email;
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            var pessoa = ObterPessoaSelecionada(sender);

            ShowMessageQuestion($"Excluir {pessoa.Nome}?", () => { _pessoaService.Delete(pessoa); CarregarLista(); });
        }

        private Pessoa ObterPessoaSelecionada(object sender)
        {
            var parent = (sender as Button).Parent;
            var stackPanelAtual = parent as StackPanel;
            var pessoa = stackPanelAtual.DataContext as Pessoa;

            return pessoa;
        }
    }
}
