using System;
using System.Windows;

namespace Database.WPF.Infrastructure
{

    public class MessageBoxService : Utility.Common.Base.IMessageBoxService
    {
        public bool? ShowMessage(Utility.Common.Base.MessageBoxConfiguration messageBoxConfiguration)
        {
            var result = MessageBox.Show(
                messageBoxConfiguration.Body,
                messageBoxConfiguration.Title,
                Map(messageBoxConfiguration.MessageBoxButton),
                Map(messageBoxConfiguration.MessageBoxImage));

            return Map(messageBoxConfiguration.MessageBoxButton, result);
        }

        private bool? Map(Utility.Common.Base.MessageBoxButton button, MessageBoxResult result)
        {
            switch ((button, result))
            {
                case (Utility.Common.Base.MessageBoxButton.OK, MessageBoxResult.OK):
                case (Utility.Common.Base.MessageBoxButton.OKCancel, MessageBoxResult.OK):
                case (Utility.Common.Base.MessageBoxButton.YesNo, MessageBoxResult.Yes):
                case (Utility.Common.Base.MessageBoxButton.YesNoCancel, MessageBoxResult.Yes):
                    return true;
                case (Utility.Common.Base.MessageBoxButton.YesNoCancel, MessageBoxResult.Cancel):
                    return null;
                default:
                    return false;
            }
        }

        private MessageBoxButton Map(Utility.Common.Base.MessageBoxButton button)
        {
            switch (button)
            {
                case Utility.Common.Base.MessageBoxButton.OK:
                    return MessageBoxButton.OK;
                case Utility.Common.Base.MessageBoxButton.OKCancel:
                    return MessageBoxButton.OKCancel;
                case Utility.Common.Base.MessageBoxButton.YesNo:
                    return MessageBoxButton.YesNo;
                case Utility.Common.Base.MessageBoxButton.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
                default:
                    throw new Exception("sdf_938sd");
            }
        }

        private MessageBoxImage Map(Utility.Common.Base.MessageBoxImage button)
        {

            if (button == Utility.Common.Base.MessageBoxImage.Asterisk)
                return MessageBoxImage.Asterisk;
            else if (button == Utility.Common.Base.MessageBoxImage.Error)
                return MessageBoxImage.Error;
            else if (button == Utility.Common.Base.MessageBoxImage.Exclamation)
                return MessageBoxImage.Exclamation;
            else if (button == Utility.Common.Base.MessageBoxImage.Hand)
                return MessageBoxImage.Hand;
            else if (button == Utility.Common.Base.MessageBoxImage.Information)
                return MessageBoxImage.Information;
            else if (button == Utility.Common.Base.MessageBoxImage.None)
                return MessageBoxImage.None;
            else if (button == Utility.Common.Base.MessageBoxImage.Question)
                return MessageBoxImage.Question;
            else if (button == Utility.Common.Base.MessageBoxImage.Stop)
                return MessageBoxImage.Stop;
            else if (button == Utility.Common.Base.MessageBoxImage.Warning)
                return MessageBoxImage.Warning;

            throw new Exception("334sdf_938sd");
        }
    }
}
