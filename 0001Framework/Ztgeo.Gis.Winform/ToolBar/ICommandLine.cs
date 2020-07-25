using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztgeo.Gis.Winform.Command
{
	public interface ICommandLine
	{
		string CommandInfomation { get; set; }

		IList<object> BindingObject { get; set; }

		void OnCreate(object hook); 

		void ActiveCommand();

		void CancelExcute(bool ByTool);

		void FinishExcute(bool ByTool);

		string CommandLineName { get; set; }

		string CommandLineShortName { get; set; }

		int CommandLineStepCount { get; set; }

		int CurrentStep { get; set; }

		bool IsExecutedCommandLine { get; }

		string CommandLineFullName { get; }

		bool NextStep(string inputString, bool ByTool);

		string CommandLineDescription { get; }

		bool CanExecute { get; }

		string CanNotExecuteInfomation { get; }

		enumCommandLineState CommandLineState { get; }

		string CommandLineCategory { get; }

		void PushToNextStep(bool ByTool);

		void BackToPreviousStep(bool ByTool);
	}
}
