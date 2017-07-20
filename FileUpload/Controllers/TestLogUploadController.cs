using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Principal;
using System.Security.Permissions;
using System.Linq;
using Utilities.Helpers;

namespace FileUpload.Controllers
{
    public class TestLogUploadController : ApiController
    {
        public TestLogUploadController()
        {
       
            
        }

        [Route("api/TestResultUpload/TestMatrix/{testMatrixId}/TestTracker/{testTrackerDmsId}/TestConfiguration/{testConfigId}/")]
        public HttpResponseMessage Post(int testMatrixId, int testTrackerDmsId, int testConfigId)
        {
            var testPlan = _getTestPlanByTestMatrixId(testMatrixId);
            var testTracker = _getTestTrackerByDmsId(testTrackerDmsId);
            var testConfig = _getTestConfigByTestConfigId(testConfigId);

            var pathToFiles = _generatePath(testPlan, testTracker, testConfig);
            _saveFiles(pathToFiles);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private string _generatePath(dynamic testPlan, dynamic testTracker, dynamic testConfig)
        {
            // var baseLogServerPath = @"\\wdscnac6040b.sc.wdc.com\WDSC-ESGSTOR\WDSC-ESG\";
            var baseLogServerPath = @"\\sitirvstorage\esguser\";
            var path = $@"{testPlan.Customer}\{testPlan.Program}\{testPlan.FirmwareBuild}_{testPlan.HardwareBuild}\{testPlan.Name}_{testPlan.Phase}\{testTracker.testProcedureName}_{testPlan.TestPlanDmsId}\{testConfig.name}_{testConfig.id}\";

            // if the path exists and is already created, then we should check to see if there is a run # and if 
            // that run # exists, let's increment it and store the files in a incremented directory
            var fullPath = baseLogServerPath + path;

            // get all directories inside
            string runNumber;
            using (new Impersonation("sc", "esguser", "esglab"))
            {

                if (!Directory.Exists(fullPath))
                {
                    runNumber = @"1\";
                }
                else
                {
                    var allDirectories = Directory.GetDirectories(fullPath).Select(x => Path.GetFileName(x));
                    var highest = allDirectories.Max(a => Convert.ToInt32(a));
                    runNumber = $@"{highest + 1}\";
                }
            }

            return fullPath + runNumber;
        }

        private object _getTestConfigByTestConfigId(int testConfigId)
        {
            return new
            {
                id = testConfigId,
                name = "CTPM240ML"
            };
        }

        private object _getTestTrackerByDmsId(int testTrackerDmsId)
        {
            return new
            {
                dmsId = testTrackerDmsId,
                testProcedureName = "Overnight Power Cycle"
            };
        }

        private void _saveFiles(string pathToFiles)
        {
            using (new Impersonation("sc", "esguser", "esglab"))
            {

                var httpRequest = HttpContext.Current.Request;

                var uploadPath = pathToFiles;

                Directory.CreateDirectory(uploadPath);

                var fullPath = httpRequest.Form["fullPath"];

                foreach (string fileKeyName in httpRequest.Files)
                {
                    HttpPostedFile file = httpRequest.Files[fileKeyName];

                    string filename = fullPath != null ? fullPath : file.FileName;
                    string extension = file.ContentType;

                    var curFolderPath = uploadPath;
                    while (filename.Contains("/"))
                    {
                        var indexOf = filename.IndexOf('/');
                        curFolderPath += "\\" + filename.Substring(0, indexOf);
                        Directory.CreateDirectory(curFolderPath);
                        filename = filename.Substring(indexOf + 1);
                    }

                    if (!string.IsNullOrEmpty(filename))
                    {
                        extension = Path.GetExtension(filename);

                        //var renamedFile = type;

                        file.SaveAs(curFolderPath + $@"\{filename}");
                    }
                }
            }
        }

        private object _getTestPlanByTestMatrixId(int testMatrixId)
        {
            var testMatrix = _getTestMatrixById(testMatrixId);
            // return testMatrix;
            // get fusion detials by test plan id

            return new
            {
                TestPlanDmsId = 123,
                Name = "TESTPLANNAME",
                Phase = "DVT",
                Customer = "Cisco",
                Program = "VELA-A",
                HardwareBuild = "LRU",
                FirmwareBuild = "08.C7",
                EngineeringChangeDescription = "ENG"
            };
        }

        private object _getTestMatrixById(int testMatrixId)
        {
            return null;
        }
    }
}
