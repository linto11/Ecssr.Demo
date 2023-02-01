using AutoMapper;
using Ecssr.Demo.Application.Common.Enums;
using Ecssr.Demo.Application.Common.Exceptions;
using Ecssr.Demo.Application.Entities;
using Ecssr.Demo.Common;
using Ecssr.Demo.Common.Utility;
using Ecssr.Demo.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System.Drawing;

namespace Ecssr.Demo.Application.UseCases.News.DownloadNewsDetail
{
    #region Request
    public class Request : IRequest<DownloadDetail>
    {
        public string Id { get; set; }
        public DownloadFormat DownloadFormat { get; set; }
        public int FontSize { get; set; }

    }
    #endregion

    #region Handler
    /// <summary>
    /// This class handles the functionality to download the news detail and return the file based on the requested format
    /// </summary>
    public class Handler : BaseUseCaseHandler, IRequestHandler<Request, DownloadDetail>
    {
        private readonly INewsDbContext _newsDbContext;
        private readonly IFileTemplate _fileTemplate;

        /// <summary>
        /// Constructor to initlize default objects
        /// </summary>
        /// <param name="refId">Ref Id of the request</param>
        /// <param name="mapper">Mapper object to map from DB to client or vice versa</param>
        /// <param name="appSetting">App Setting object. To be used if required</param>
        /// <param name="newsDbContext">DB Context to access news table</param>
        /// <param name="fileTemplate">Template to create the file</param>
        public Handler(IRefId refId, IMapper mapper, AppSetting appSetting,
            INewsDbContext newsDbContext, IFileTemplate fileTemplate) : base(refId, mapper, appSetting)
        {
            _newsDbContext = newsDbContext;
            _fileTemplate = fileTemplate;
        }

        /// <summary>
        /// This method is the main function which handles the logic for generating and returning the file in the required format.
        /// </summary>
        /// <param name="request">Paylod required for the function to process.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DownloadDetail> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                //get the news based on Id
                var dbNews = await _newsDbContext.News.Include(n => n.NewsDownloads).FirstOrDefaultAsync(n => n.Id == request.Id);
                if (dbNews != null)
                {
                    //update counter into db, create newsdownload object
                    if (dbNews.NewsDownloads == null)
                        dbNews.NewsDownloads = new List<Infrastructure.Persistence.Models.NewsDownload>();

                    string base64 = null;
                    switch (request.DownloadFormat)
                    {
                        case DownloadFormat.A4Pdf:
                            {
                                //generate base64 pdf
                                base64 = GeneratePdfImage(dbNews, request.FontSize, request.DownloadFormat);

                                //update the counter
                                var a4PdfDownload = dbNews.NewsDownloads.FirstOrDefault(nd => nd.DownloadFormat == DownloadFormat.A4Pdf.ToString());
                                if (a4PdfDownload == null)
                                {
                                    a4PdfDownload = new Infrastructure.Persistence.Models.NewsDownload
                                    {
                                        DownloadFormat = DownloadFormat.A4Pdf.ToString(),
                                        Count = 1,
                                        News = dbNews
                                    };

                                    dbNews.NewsDownloads.Add(a4PdfDownload);
                                }
                                else
                                    a4PdfDownload.Count++;
                            }
                            break;
                        case DownloadFormat.MobilePdf:
                            {
                                //generate base 64
                                base64 = GeneratePdfImage(dbNews, request.FontSize, request.DownloadFormat);

                                //update the counter
                                var mobilePdfDownload = dbNews.NewsDownloads.FirstOrDefault(nd => nd.DownloadFormat == DownloadFormat.MobilePdf.ToString());
                                if (mobilePdfDownload == null)
                                {
                                    mobilePdfDownload = new Infrastructure.Persistence.Models.NewsDownload
                                    {
                                        DownloadFormat = DownloadFormat.MobilePdf.ToString(),
                                        Count = 1,
                                        News = dbNews
                                    };

                                    dbNews.NewsDownloads.Add(mobilePdfDownload);
                                }
                                else
                                    mobilePdfDownload.Count++;
                            }
                            break;
                        case DownloadFormat.MobileImage:
                            {
                                //generate base 64
                                base64 = GeneratePdfImage(dbNews, request.FontSize, request.DownloadFormat);

                                //update the counter
                                var mobileImageDownload = dbNews.NewsDownloads.FirstOrDefault(nd => nd.DownloadFormat == DownloadFormat.MobileImage.ToString());
                                if (mobileImageDownload == null)
                                {
                                    mobileImageDownload = new Infrastructure.Persistence.Models.NewsDownload
                                    {
                                        DownloadFormat = DownloadFormat.MobileImage.ToString(),
                                        Count = 1,
                                        News = dbNews
                                    };

                                    dbNews.NewsDownloads.Add(mobileImageDownload);
                                }
                                else
                                    mobileImageDownload.Count++;
                            }
                            break;
                    }

                    await _newsDbContext.SaveChangesAsync(CancellationToken.None);
                    
                    return new DownloadDetail { NewsDownloads = Mapper.Map<IList<NewsDownload>>(dbNews.NewsDownloads), FileBase64 = base64, };
                }
                else
                    NotFoundException.Throw(Common.Constants.Message.News.DownloadNewsDetail.Failure.NotFound,
                        Common.Enums.ErrorNumber.NewsDeailNotFound);
            }
            catch (NotFoundException ex) { throw ex; }
            catch (UnprocessableEntityException ex) { throw ex; }
            catch (Exception ex)
            {
                InternalServerErrorException.Throw(RefId, Common.Constants.Message.News.DownloadNewsDetail.Failure.InternalServerError, ex);
            }

            return default;
        }

        /// <summary>
        /// This methods is used to generate the pdf / image and download as required
        /// </summary>
        /// <param name="dbNews">the news detail object</param>
        /// <param name="fontSize">font size requried in the file</param>
        /// <param name="downloadFormat">format of the file to be generated and downloaded</param>
        /// <returns></returns>
        private string GeneratePdfImage(Infrastructure.Persistence.Models.News dbNews, int fontSize, DownloadFormat downloadFormat)
        {
            string html = null;
            string base64 = null;

            switch (downloadFormat)
            {
                case DownloadFormat.A4Pdf:
                    {
                        html = ReplaceText(_fileTemplate.A4, dbNews, fontSize, downloadFormat);

                        var converter = new HtmlToPdf();

                        converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                        converter.Options.PdfPageSize = PdfPageSize.A4;
                        converter.Options.MarginLeft = 25;
                        converter.Options.MarginRight = 25;
                        converter.Options.EmbedFonts = true;

                        PdfDocument doc = converter.ConvertHtmlString(html);
                        var pdfbytes = doc.Save();

                        //convert to base64
                        base64 = Convert.ToBase64String(pdfbytes, 0, pdfbytes.Length);

                        doc.Close();
                    }
                    break;
                
                case DownloadFormat.MobilePdf:
                case DownloadFormat.MobileImage:
                    {
                        html = ReplaceText(_fileTemplate.Mobile, dbNews, fontSize, downloadFormat);
                        if(downloadFormat == DownloadFormat.MobilePdf)
                        {
                            var converter = new HtmlToPdf();

                            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                            converter.Options.PdfPageSize = PdfPageSize.Custom;
                            converter.Options.MarginLeft = 30;
                            converter.Options.MarginRight = 30;
                            converter.Options.MarginTop = 20;
                            converter.Options.MarginBottom = 20;
                            converter.Options.EmbedFonts = true;

                            PdfDocument doc = converter.ConvertHtmlString(html);
                            var pdfbytes = doc.Save();

                            //convert to base64
                            base64 = Convert.ToBase64String(pdfbytes, 0, pdfbytes.Length);

                            doc.Close();
                        }
                        else if(downloadFormat == DownloadFormat.MobileImage)
                        {
                            // instantiate a html to image converter object
                            HtmlToImage imgConverter = new HtmlToImage();

                            // create a new image converting an html body
                            Image image = imgConverter.ConvertHtmlString(html);

                            Bitmap bitmap = (Bitmap)image;
                            MemoryStream memoryStream = new MemoryStream();
                            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                            byte[] imageBytes = memoryStream.ToArray();

                            base64 = Convert.ToBase64String(imageBytes);
                        }
                    }
                    break;
            }

            return base64;
        }

        /// <summary>
        /// Replace the text inside the template before generating the tempalte
        /// </summary>
        /// <param name="template">the template in which the string has to be manipulated</param>
        /// <param name="dbNews">News object</param>
        /// <param name="fontSize">font size that needs to be replaced</param>
        /// <param name="downloadFormat">the format of the file to be downloaded</param>
        /// <returns></returns>
        private string ReplaceText(string template, Infrastructure.Persistence.Models.News dbNews, int fontSize, DownloadFormat downloadFormat)
        {
            template = template.Replace("###imageUrl###", !dbNews.ImageUrl.IsNullOrEmpty() ? dbNews.ImageUrl : Common.Constants.Default.NoImageUrl);
            template = template.Replace("###title###", dbNews.Title);
            template = template.Replace("###fontSize###", fontSize.ToString());
            template = template.Replace("###detail###", dbNews.Detail);
            template = template.Replace("###formatType###", downloadFormat.ToString());

            return template;
        }
    }
    #endregion
}

