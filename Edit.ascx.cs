//
// Christoc.com - http://www.christoc.com
// Copyright (c) 2014-2019
// by Christoc.com
//
// Originally licensed by
// DotNetNuke - http://www.dotnetnuke.com
// Copyright (c) 2002-2012
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using Christoc.Modules.dnnsimplearticle.Components;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common;


//TODO: create edit interface for Thumbnail and Large images
//TODO: implement Thumbnail and Large image views

namespace Christoc.Modules.dnnsimplearticle
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Editdnnsimplearticle class is used to manage content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : dnnsimplearticleModuleBase
    {

        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Load += Page_Load;

            DotNetNuke.Framework.jQuery.RequestUIRegistration();
            DotNetNuke.Framework.jQuery.RequestDnnPluginsRegistration();


        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //TODO: We need to check if a permalink is taken before allowing them to save

                if (!Page.IsPostBack)
                {

                    var article = new Article();
                    tsTerms.Terms = article.Terms;
                    article = ArticleController.GetArticle(ArticleId);

                    tsTerms.PortalId = PortalId;

                    urlImage.UrlType = "F";
                    urlImage.FileFilter = "jpg,jpeg,jpe,gif,bmp,png";

                    if (article != null)
                    {
                        txtTitle.Text = article.Title;
                        txtDescription.Text = article.Description;
                        txtBody.Text = article.Body;
                        tsTerms.Terms = article.Terms;
                        urlImage.Url = article.LargeImg;
                        txtPermaLink.Text = article.PermaLink;
                    }

                    tsTerms.DataBind();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        protected void LbSaveClick(object sender, EventArgs e)
        {
            Article a;
            if (ArticleId > 0)
            {
                a = ArticleController.GetArticle(ArticleId);
                a.Title = txtTitle.Text.Trim();
                a.Description = txtDescription.Text.Trim();
                a.Body = txtBody.Text;
                a.LastModifiedOnDate = DateTime.Now;
                a.LastModifiedByUserId = UserInfo.UserID;
                a.ModuleId = ModuleId;
                a.LargeImg = urlImage.Url;
                a.PermaLink = txtPermaLink.Text.Trim();

            }
            else
            {
                a = new Article
                {
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Body = txtBody.Text,
                    CreatedOnDate = DateTime.Now,
                    CreatedByUserId = UserInfo.UserID,
                    LastModifiedOnDate = DateTime.Now,
                    LastModifiedByUserId = UserInfo.UserID,
                    ModuleId = ModuleId,
                    LargeImg = urlImage.Url,
                    PermaLink = txtPermaLink.Text.Trim()
                };
            }

            if (tsTerms.Terms != null)
            {
                a.Terms.Clear();
                a.Terms.AddRange(tsTerms.Terms);
            }
            a.Save(TabId);
            Response.Redirect(Globals.NavigateURL(TabId));
        }

        protected void LbCancelClick(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(TabId));
        }
    }
}