   M S B A M L   `   `   `  ����B  >ChatApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ChatAppChatApp  5          ChatApp.Page1   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'localclr-namespace:ChatApp     5        N� Title$  Page1��5	      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��124* �6   �� 5      $	��39* �6   �� 5      $
��137* �6   .+�5   
   �� $	��434��6   $	��Top=�6"   $��3��6:   .+�5      �� -   5         ��Name$ viewMoreBtn��5      $	��...��6*   $��28��68   $��52��6D   $��Right=�6O   6k   �� 5      -   5         ��Name$ chatListBox��5      $	��304��6+   $	��535��68   $��40,20,10,10q�6D   $
��Left=�6Y   $
��Auto=�6t   ��5      �� 5      .R�5      �� .+�5      �� $
��Left=�6@   
����  ���6(   �� *Sender+6[   �� 5      ��6(   �� *	Message+��5   "   �� 5   &    �� 
TargetType" ��6,   .�5   *   -   5        5   '   5      5      5       5"      � 5#      $��45                   ��6    5%      �� 5&      $
��Auto �6'   �� 5(      $��* �6'   .+�5+      �� -   5         ��Name$ 	messageTB��5+      $��0��6-   $	��380��6=    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6^   $��2��6r   $��
40,0,180,0q�6�   �� 5,      -   5        $ sendBtn��5,      $��1��6*   $
��SEND��6:   $��70��6I   $��
436,0,94,0q�6k    J� RenderTransformOrigin$ 0.306,0.488&�6   �� 5-      -   5        $ sendFileBtn��5-      $��1��6.   $��FILES��6>   $��64��6N   $ 1.749,0.589&�6Y   $��
511,0,25,0q�6}   6�   5/      50      � 51   
   -   5        	 �Name$	 	sidePanel��51      $��2��6"   $	��150��62   ���	#FFF3CB83$��	117,0,7,0q�6U   $y�	Collapsed=�6h   .+�53      �� -   5        $��Close��53      $��Right=�6<   $	��Top=�6X   $��
0,10,100,0q�6p   ���	#FFF19A91���	#FFF9F5F56�   �� 54      -	   5        
 ��Name$
 sideStackPanel��54       �� Orientation$ Vertical=�61   $��	0,82,0,55q�6H   57      �� 58      $��Group Member List :��6   $��Center=�62   $��0,42,0,0q�6O   $	��Top=�6a   $��Bodoni MT Black)�6y   �� 59      -
   5        $ leaveGroupBtn��59      $��Leave Group��6,   $��Center=�6B   $��	0,391,0,0q�6_   $	��Top=�6r   $	��106��6�   ���	#FFF91111���	#FFFDF9F96�   5;      5<      5=      