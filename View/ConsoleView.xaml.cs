/*
 * Created by SharpDevelop.
 * User: Admin Security
 * Date: 10/10/15
 * Time: 9:39 PM
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Windows.Controls;

namespace BloodAlcoholCalculator.View
{
     /// <summary>
     /// Interaction logic for ConsoleView.xaml
     /// </summary>
     public partial class ConsoleView : UserControl
     {
          #region Constructors

          public ConsoleView()
          {
               InitializeComponent();
          }

          #endregion Constructors

          #region Methods

          private void History_TextChanged(object sender, TextChangedEventArgs e)
          {
               ScrollViewerHistory.ScrollToBottom();
          }

          #endregion Methods
     }
}