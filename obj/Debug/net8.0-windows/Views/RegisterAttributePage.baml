   M S B A M L   `   `   `  ����J  FSmartHealthTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:SmartHealthTestSmartHealthTest  5   	   1    +SmartHealthTest.Views.RegisterAttributePage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:SmartHealthTest     5   	     N� Title$  属性登録��5   	   $
��Auto��6   $
��Auto��6$   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��属性登録��6   $��22(�6    $
��Bold"�6.   $��10q�6@      PrimaryBrush#���   6L   $��30��6x   � 5   
   $��1��6    5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      � $��0��6   $��
80,10,50,0q�6    $��35��64   $��Center=�6@    � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   .+�5       �� $��0��6   $��
都市OSID��6,   $��Center=�6:   $
��Bold"�6U   $��16(�6g   $��75��6u   #���   6�   �� 5!      -   5         ��Name$ searchID��5!   f   $��1��6   $	��430��6*   $��16(�66   $��Center=�6D   $��0 0 5 0q�6x     DataGridBrush#���  6�     AlternateBrush#���  6�   �� 5"      -   5        $��2��5"      $��60��6)   $��検索��64   $��16(�6A   $��0 0 5 0q�6O   6`   �� 5#      -   5        $��3��5#      $��60��6)     
ClearStyle#���  64   $��	クリア��6Y   $��16(�6g   $��0 0 5 0q�6v   6�   5$      �� 5&      -   5         ��Name$ 
gridScroll��5&   D   $��1��6   $��Hidden=�6   .��5'      � -   5         �Name$ _attributeNames��5'      5(      5)      � 5+   
   -   5        $ 
NoDataGrid��5+      $��2��6#   $��80, 10, 50, 0q�60   $y�Visible=�6H    5,      �� 5-      $��50 �6    �� 5.      $
��Auto �6    .+�50      � -   5        $ _headerGrid��50   L   $��0��6   #���   6    6L   � 51      -   5        $ 	_bodyGrid��51       $��1��6   6    52      �� 54   
   -	   5        $��3��54      $��登録��6   $	��100��6+   $��50��67   $��Right=�6C   $��0, 10, 50, 50q�6_   $��16(�6v   6�   55      56      