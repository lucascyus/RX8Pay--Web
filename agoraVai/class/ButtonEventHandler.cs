
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public class ButtonEventHandler
{
    private HttpResponse response;

    public ButtonEventHandler(HttpResponse response)
    {
        this.response = response;
    }

    public void EditarFuncionario_Click(object sender, EventArgs e)
    {
        // Lógica para manipular o evento "Editar" aqui
    }

    public void ExportarFuncionario_Click(object sender, EventArgs e)
    {
        Document doc = new Document();
        MemoryStream memoryStream = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
        doc.Open();

        // Adicione o conteúdo ao PDF
        doc.Add(new Paragraph("Este é um exemplo de PDF gerado a partir de um botão em uma página da web."));

        doc.Close();

        // Defina os cabeçalhos para informar o navegador que é um arquivo PDF
        response.ContentType = "application/pdf";
        response.AddHeader("content-disposition", "attachment;filename=exemplo.pdf");

        // Envie o conteúdo do PDF gerado
        response.OutputStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
        response.Flush();
        response.End();
        response.Write("O botão foi clicado!");
    }
}

