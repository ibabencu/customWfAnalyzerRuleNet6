using UiPath.Studio.Activities.Api;
using UiPath.Studio.Activities.Api.Analyzer;
using UiPath.Studio.Activities.Api.Analyzer.Rules;
using UiPath.Studio.Analyzer.Models;

namespace customWfAnalyzerRuleNet6
{
    public class RuleRepositoryNet60 : IRegisterAnalyzerConfiguration
    {
        public void Initialize(IAnalyzerConfigurationService workflowAnalyzerConfigService)
        {
            if (!workflowAnalyzerConfigService.HasFeature("WorkflowAnalyzerV4"))
                return;

            var forbiddenStringRule = new Rule<IActivityModel>("net60RuleNotAllowedVar", "DE-USG-002", InspectVariableForString);
            forbiddenStringRule.DefaultErrorLevel = System.Diagnostics.TraceLevel.Warning;
            forbiddenStringRule.Parameters.Add("string_in_variable", new Parameter()
            {
                DefaultValue = "net60",
                Key = "net60RuleNotAllowedVar",
                LocalizedDisplayName = "Illegal string net 6.0"
            }
                );

            workflowAnalyzerConfigService.AddRule<IActivityModel>(forbiddenStringRule);

        }
        private InspectionResult InspectVariableForString(IActivityModel activityToInspect, Rule configuredRule)
        {
            var configuredString = configuredRule.Parameters["string_in_variable"]?.Value;
            if (string.IsNullOrEmpty(configuredString))
            {
                return new InspectionResult { HasErrors = false };
            }

            //checking to see if we have any variables
            if (activityToInspect.Variables.Count == 0)
            {
                return new InspectionResult { HasErrors = false };
            }

            //creating a list of messages to return if the rule is broken
            var messageList = new List<InspectionMessage>();

            foreach (var variable in activityToInspect.Variables)
            {
                //if variable name contains the string that breaks the rule, we add the message to the error message list of the wf analyzer
                if (variable.DisplayName.Contains(configuredString))
                {
                    messageList.Add(new InspectionMessage()
                    {
                        Message = $"Variable {variable.DisplayName} contains an illegal string {configuredString}"
                    });
                }
            }
            if (messageList.Any())
            {
                return new InspectionResult()
                {
                    HasErrors = true,
                    InspectionMessages = messageList,
                    RecommendationMessage = "Variable contains illegal string, please fix net 6.0",
                    ErrorLevel = configuredRule.ErrorLevel
                };
            }
            else return new InspectionResult { HasErrors = false };
        }
    }
}
