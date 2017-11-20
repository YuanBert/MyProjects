/**
  ******************************************************************************
  * File Name          : gpio.c
  * Description        : This file provides code for the configuration
  *                      of all used GPIO pins.
  ******************************************************************************
  ** This notice applies to any and all portions of this file
  * that are not between comment pairs USER CODE BEGIN and
  * USER CODE END. Other portions of this file, whether 
  * inserted by the user or by software development tools
  * are owned by their respective copyright owners.
  *
  * COPYRIGHT(c) 2017 STMicroelectronics
  *
  * Redistribution and use in source and binary forms, with or without modification,
  * are permitted provided that the following conditions are met:
  *   1. Redistributions of source code must retain the above copyright notice,
  *      this list of conditions and the following disclaimer.
  *   2. Redistributions in binary form must reproduce the above copyright notice,
  *      this list of conditions and the following disclaimer in the documentation
  *      and/or other materials provided with the distribution.
  *   3. Neither the name of STMicroelectronics nor the names of its contributors
  *      may be used to endorse or promote products derived from this software
  *      without specific prior written permission.
  *
  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
  * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
  * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
  * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
  * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
  * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
  * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  *
  ******************************************************************************
  */

/* Includes ------------------------------------------------------------------*/
#include "gpio.h"
/* USER CODE BEGIN 0 */

/* USER CODE END 0 */

/*----------------------------------------------------------------------------*/
/* Configure GPIO                                                             */
/*----------------------------------------------------------------------------*/
/* USER CODE BEGIN 1 */

/* USER CODE END 1 */

/** Configure pins as 
        * Analog 
        * Input 
        * Output
        * EVENT_OUT
        * EXTI
*/
void MX_GPIO_Init(void)
{

  GPIO_InitTypeDef GPIO_InitStruct;

  /* GPIO Ports Clock Enable */
  __HAL_RCC_GPIOC_CLK_ENABLE();
  __HAL_RCC_GPIOD_CLK_ENABLE();
  __HAL_RCC_GPIOA_CLK_ENABLE();
  __HAL_RCC_GPIOB_CLK_ENABLE();

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(CommunicationLED_GPIO_Port, CommunicationLED_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOA, MCUAtmosphereLED1_Pin|MCUAtmosphereLED2_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOB, CTR485_EN1_Pin|RunningLED_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pin : PtPin */
  GPIO_InitStruct.Pin = CommunicationLED_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(CommunicationLED_GPIO_Port, &GPIO_InitStruct);

  /*Configure GPIO pins : PAPin PAPin */
  GPIO_InitStruct.Pin = MCUAtmosphereLED1_Pin|MCUAtmosphereLED2_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);

  /*Configure GPIO pins : PCPin PCPin */
  GPIO_InitStruct.Pin = GentleSense_Pin|MotorVerticalLimitInf_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_IT_RISING;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  HAL_GPIO_Init(GPIOC, &GPIO_InitStruct);

  /*Configure GPIO pins : PBPin PBPin */
  GPIO_InitStruct.Pin = CTR485_EN1_Pin|RunningLED_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

  /*Configure GPIO pin : PtPin */
  GPIO_InitStruct.Pin = MotorHorizontalLimitInf_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_IT_RISING;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  HAL_GPIO_Init(MotorHorizontalLimitInf_GPIO_Port, &GPIO_InitStruct);

}

/* USER CODE BEGIN 2 */
	 void BSP_RUNNINGLED_ON(void)
	 {
			HAL_GPIO_WritePin(RunningLED_GPIO_Port, RunningLED_Pin,GPIO_PIN_RESET);
	 }
	 
	 void BSP_RUNNINGLED_OFF(void)
	 {
			HAL_GPIO_WritePin(RunningLED_GPIO_Port, RunningLED_Pin,GPIO_PIN_SET);
	 }
	 
	 void BSP_RUNNINGLED_TORGGLE(void)
	 {
			HAL_GPIO_TogglePin(RunningLED_GPIO_Port,RunningLED_Pin);
	 }
	 
	 void BSP_COMMUNICATIONLED_ON(void)
	 {
			HAL_GPIO_WritePin(CommunicationLED_GPIO_Port, CommunicationLED_Pin,GPIO_PIN_RESET);
	 }
	 void BSP_COMMUNICATIONLED_OFF(void)
	 {
			HAL_GPIO_WritePin(CommunicationLED_GPIO_Port, CommunicationLED_Pin,GPIO_PIN_SET);
	 }
	 void BSP_COMMUNICATIONLED_TORGGLE(void)
	 {
			HAL_GPIO_TogglePin(CommunicationLED_GPIO_Port, CommunicationLED_Pin);
	 }
	 
	 /***************************************************************************/
	 void BSP_ATMOSPHERELED1_ON(void)
	 {
			HAL_GPIO_WritePin(MCUAtmosphereLED1_GPIO_Port,MCUAtmosphereLED1_Pin,GPIO_PIN_RESET);
	 }
	 void BSP_ATMOSPHERELED1_OFF(void)
	 {
			HAL_GPIO_WritePin(MCUAtmosphereLED1_GPIO_Port,MCUAtmosphereLED1_Pin,GPIO_PIN_SET);
	 }
	 void BSP_ATMOSPHERELED1_TOGGLE(void)
	 {
			HAL_GPIO_TogglePin(MCUAtmosphereLED1_GPIO_Port,MCUAtmosphereLED1_Pin);
	 }
	 
	 void BSP_ATMOSPHERELED2_ON(void)
	 {
			HAL_GPIO_WritePin(MCUAtmosphereLED2_GPIO_Port,MCUAtmosphereLED2_Pin,GPIO_PIN_RESET); 
	 }
	 void BSP_ATMOSPHERELED2_OFF(void)
	 {
			HAL_GPIO_WritePin(MCUAtmosphereLED2_GPIO_Port,MCUAtmosphereLED2_Pin,GPIO_PIN_SET);	 
	 }
	 void BSP_ATMOSPHERELED2_TOGGLE(void)
	 {
			HAL_GPIO_TogglePin(MCUAtmosphereLED2_GPIO_Port,MCUAtmosphereLED2_Pin);	 
	 }

/* USER CODE END 2 */

/**
  * @}
  */

/**
  * @}
  */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
