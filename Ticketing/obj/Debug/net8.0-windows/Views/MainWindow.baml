   M S B A M L   `   `   `  ����D  @Ticketing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Ticketing.ViewModelsTicketing.ViewModels  5   	       Ticketing.Views.MainView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   9
viewModels"clr-namespace:Ticketing.ViewModels     5   	     � Title$  Problemus Maxima��5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowState$ 	Maximized=�5
   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � DataContext 5      (   "Ticketing.ViewModels.MainViewModel  5   
    � 	Resources 5         RoundedButtonStyle �� 
TargetType �� Property	 �� Value
 �� 
TargetType �� CornerRadius J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth   Opacity  CloseButtonStyle �� Padding��%  &	        &	 D    �� 5   
   " ��6+   .�5      ��  � ��	 5      �� 5      "
 ��6&   .R�5      �� $ 15��6!   ���#8B5E3C���#BF8A60$��1q�6^   ._�5      �� $��Center=�6/   $��Center=�6L   $��20,10q�6g    5        5   "   $ Black��63   $ 10Q�6A   $ 5Q�6Q   $	 0.5Q�6a   5      5      5      �� 5        � ��
	 �����6+   �� 5!       � ��$	 22(�6)   �� 5"       � ��$		 550��6&   �� 5#       � ��$	 80��6'   �� 5$       � ��$	 15q�6'   �� 5%       �@�Cursor$
	 Hand��6'   5&      �� 5(   
   " ��6)   .�5)      ��  � ��	 5*      �� 5+      "
 ��6&   .R�5,      �� 
������ $ 5q�6:   ._�5-      �� $	��✕��6(   
�������$��18(�6D   5.      5/      51      �� 52       � ��$	 30��6&   �� 53       � ��$	 30��6'   �� 54       � ��$	 Right=�64   �� 55       � ��$		 Top=�62   �� 56       � ��$	 0q�6'   �� 57       �@�Cursor$
	 Hand��6'   58      59      .��5;      � ��5<   
   �� 5=       �� ImageSource$ /Resources/game-5.png��6    j� Stretch$ UniformToFill=�6=    � RowDefinitions 5@   
   �� 5A      $��80 �6   �� 5B      $��* �6   .+�5E   
   ]� $��0��6   $��False��6!   ���#0D1B3E.+�5F      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�62   $��Center=�6H   $
��20,0q�6c   .+�5G      �� $��/Resources/Group.png��6   $��50��66   $��50��6A   $��5q�6M   �� 5H      $��Problemus Maxima��6   $��Center=�5I      
�������$��32(�5K      $
��Bold"�5L        	MagicFont#��[ 5M      5N      �� 5P      -   5        #��[ 5P      $��Right=�5Q      $��0,0,20,0q�5R      5S      5T      �� 5V   
   $��1��6   $ Vertical=�5W      $��Center=�5X      $��Center=�5Y      .+�5[      �� $%��Bienvenue dans le monde magique��6   
�������$��40(�5]      #��[ 5^      $��Center=�5_      $��0,0,0,40q�5`       5a        5b      $ Black��6'   $ 5Q�65   $ 3Q�6D   $	 0.7Q�6T   5d      �� 5f      $ 
Horizontal=�6   $��Center=�62   .+�5g      �� #��[  6   ��5h      �� *OuvrirTicketCommand+.��5i      �� $ 
Horizontal=�6!   $��Center=�6:   .+�5j      �� $��Lettre pour Poudlard��6$   $��26(�5k   $   #��[ 5l   $   $��Center=�5m   $   �� 5n      $*��$/Resources/solar_letter-bold (2).png��6    $��40��6N   $��40��6Y   $��15,0,0,0q�6e   5o      5p      �� 5r      #��[  6   ��5s      �� *OpenAdminLoginCommand+.��5t      �� $ 
Horizontal=�6!   $��Center=�6:   .+�5u      �� $%��Connexion au Portail du Sorcier��6$   $��26(�5v   $   #��[ 5w   $   $��Center=�5x   $   �� 5y      $��/Resources/Calque 1.png��6    $��40��6A   $��40��6L   $��15,0,0,0q�6X   5z      5{      5|      5}      �� 5   
   $��1��6   $H��B© Poudlard - Tous droits réservés par le Ministère de la Magie��5�      ���#CCCCCC$��12(�5�      $��Center=�5�      $��Bottom=�5�      $��0,0,0,15q�5�      5�      5�      