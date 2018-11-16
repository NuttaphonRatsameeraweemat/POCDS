﻿using DS.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {

        #region [Fields]

        /// <summary>
        /// The Attachment manager provides Attachment functionality.
        /// </summary>
        private readonly IAttachment _attachment;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="AttachmentController" /> class.
        /// </summary>
        /// <param name="hremployee"></param>
        public AttachmentController(IAttachment attachment)
        {
            _attachment = attachment;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile()
        {
            return Ok();
        }

        #endregion

    }
}