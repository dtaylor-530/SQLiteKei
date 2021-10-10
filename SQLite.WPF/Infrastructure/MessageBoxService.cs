using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLite.WPF.Infrastructure
{

    public class MessageBoxService : Common.IMessageBoxService
    {
        public bool? ShowMessage(Common.MessageBoxConfiguration messageBoxConfiguration)
        {
            var result = MessageBox.Show(
                messageBoxConfiguration.Body,
                messageBoxConfiguration.Title,
                Map(messageBoxConfiguration.MessageBoxButton),
                Map(messageBoxConfiguration.MessageBoxImage));

            return Map(messageBoxConfiguration.MessageBoxButton, result);
        }

        private bool? Map(Common.MessageBoxButton button, MessageBoxResult result)
        {
            switch ((button, result))
            {
                case (Common.MessageBoxButton.OK, MessageBoxResult.OK):
                case (Common.MessageBoxButton.OKCancel, MessageBoxResult.OK):
                case (Common.MessageBoxButton.YesNo, MessageBoxResult.Yes):
                case (Common.MessageBoxButton.YesNoCancel, MessageBoxResult.Yes):
                    return true;
                case (Common.MessageBoxButton.YesNoCancel, MessageBoxResult.Cancel):
                    return null;
                default:
                    return false;
            }
        }


        private MessageBoxButton Map(Common.MessageBoxButton button)
        {
            switch (button)
            {
                case Common.MessageBoxButton.OK:
                    return MessageBoxButton.OK;
                case Common.MessageBoxButton.OKCancel:
                    return MessageBoxButton.OKCancel;
                case Common.MessageBoxButton.YesNo:
                    return MessageBoxButton.YesNo;
                case Common.MessageBoxButton.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
                default:
                    throw new Exception("sdf_938sd");
            }
        }

        private MessageBoxImage Map(Common.MessageBoxImage button)
        {

            if (button == Common.MessageBoxImage.Asterisk)
                return MessageBoxImage.Asterisk;
            else if (button == Common.MessageBoxImage.Error)
                return MessageBoxImage.Error;
            else if (button == Common.MessageBoxImage.Exclamation)
                return MessageBoxImage.Exclamation;
            else if (button == Common.MessageBoxImage.Hand)
                return MessageBoxImage.Hand;
            else if (button == Common.MessageBoxImage.Information)
                return MessageBoxImage.Information;
            else if (button == Common.MessageBoxImage.None)
                return MessageBoxImage.None;
            else if (button == Common.MessageBoxImage.Question)
                return MessageBoxImage.Question;
            else if (button == Common.MessageBoxImage.Stop)
                return MessageBoxImage.Stop;
            else if (button == Common.MessageBoxImage.Warning)
                return MessageBoxImage.Warning;


            throw new Exception("334sdf_938sd");
        }
    }
}

