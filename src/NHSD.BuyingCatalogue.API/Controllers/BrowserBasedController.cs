using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetClientApplicationTypes;

namespace NHSD.BuyingCatalogue.API.Controllers
{
    [Route("api/v1/solutions")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class BrowserBasedController : ControllerBase
    {
        /// <summary>
        /// Gets the browser-based options for the client application types of a solution matching the supplied ID.
        /// </summary>
        /// <param name="id">A value to uniquely identify a solution.</param>
        /// <returns>A task representing an operation to update the details of a solution.</returns>
        [HttpGet]
        [Route("{id}/sections/browser-based")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBrowserBasedAsync([FromRoute][Required]string id)
        {
            //Canned Data
            return Ok(new BrowserBased
            {
                Sections = new List<BrowserBasedSection>
                {
                    new BrowserBasedSection { Id = "browsers-supported", Status= "COMPLETE", Requirement = "Mandatory" },
                    new BrowserBasedSection { Id = "plug-ins-or-extensions", Status= "INCOMPLETE", Requirement = "Mandatory" },
                    new BrowserBasedSection { Id = "connectivity-and-resolution", Status= "INCOMPLETE", Requirement = "Mandatory" },
                    new BrowserBasedSection { Id = "hardware-requirements", Status= "INCOMPLETE", Requirement = "Optional" },
                    new BrowserBasedSection { Id = "additional-information", Status= "INCOMPLETE", Requirement = "Optional" },
                }
            });
        }
    }

    public class BrowserBased
    {
        [JsonProperty("sections")]
        public List<BrowserBasedSection> Sections { get; set; }
    }

    public class BrowserBasedSection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("requirement")]
        public string Requirement { get; set; }
    }
}
