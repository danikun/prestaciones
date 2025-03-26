using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Prestaciones.Web.Models;

namespace Prestaciones.Web.Services;

public class DocumentoResolucionService
{
    public byte[] GenerarPdfResolucion(ReclamacionPrevia reclamacion)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Text("Resolución de Reclamación Previa")
                    .SemiBold()
                    .FontSize(20)
                    .FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(col =>
                    {
                        col.Spacing(20);

                        col.Item().Text(text =>
                        {
                            text.Span("ID de Reclamación: ").SemiBold();
                            text.Span(reclamacion.Id.ToString());
                        });

                        col.Item().Text(text =>
                        {
                            text.Span("Fecha de Entrada: ").SemiBold();
                            text.Span(reclamacion.FechaEntrada.ToLocalTime().ToString("g"));
                        });

                        col.Item().Text(text =>
                        {
                            text.Span("Estado: ").SemiBold();
                            text.Span(reclamacion.Estado);
                        });

                        col.Item().Text(text =>
                        {
                            text.Span("Fecha de Resolución: ").SemiBold();
                            text.Span(reclamacion.FechaResolucion?.ToLocalTime().ToString("g") ?? "");
                        });

                        if (!string.IsNullOrEmpty(reclamacion.MotivoResolucion))
                        {
                            col.Item().Text("Motivo de la Resolución:").SemiBold();
                            col.Item().Background(Colors.Grey.Lighten3).Padding(10).Text(reclamacion.MotivoResolucion);
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Fecha del documento: ");
                        x.Span(DateTime.Now.ToLocalTime().ToString("g"));
                    });
            });
        }).GeneratePdf();
    }
} 