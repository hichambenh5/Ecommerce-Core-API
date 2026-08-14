using Infrastructure.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce_API.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderApi : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderApi(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("All", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync()
        {
            var orderslist = await _orderService.GetAllOrdersAsync();
            if (orderslist == null || !orderslist.Any())
            {
                return NotFound("Orders not found");
            }
            return Ok(orderslist);
        }
        [HttpGet("{id}", Name = "GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDto>> GetOrderByIdAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not Actepted id: {id}");
            }
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound($"Order with id {id} Not found");
            }
            return Ok(order);
        }
        [HttpPost(Name = "AddOrderAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderCreateDto>> AddOrderAsync(OrderCreateDto dto)
        {
            if (dto == null || dto.TotalPrice <= 0 || dto.UserId <1 || dto.CouponsId <1)
            {
                return BadRequest("invalid Order Data");
            }
            var OrderCreate = await _orderService.AddOrderAsync(dto);
            if (OrderCreate <= 0)
            {
                return BadRequest("Error creating Order");
            }
            var response = new OrderCreateDto
            {
                TotalPrice = dto.TotalPrice,
                UserId = dto.UserId,
                CouponsId = dto.CouponsId
            };
            return CreatedAtRoute("GetOrderById", new { id = OrderCreate }, response);
        }
        [HttpDelete("{id}",Name = "DeleteOrderAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest("Invalid Order Data");
            }
            if(await _orderService.DeleteOrderAsync(id))
            {
                return Ok($"Order with id {id} has been deleted");
            }
            else
            {
                return NotFound($"Order with id {id} not found,no rows deleted");
            }
        }
        [HttpPut("{id}",Name = "UpdateOrderAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderUpdateDto>> UpdateOrderAsync(int id, OrderUpdateDto dto)
        {
            if(id<1 || dto==null || string.IsNullOrEmpty(dto.OrderStatus) || dto.TotalPrice <= 0)
            {
                return BadRequest("Invalid Order Data");
            }
            var Order = await _orderService.GetOrderByIdAsync(id);
            if (Order == null)
            {
                return NotFound($"Order with id {id} not found");

            }
            Order.OrderStatus = dto.OrderStatus;
            Order.TotalPrice = dto.TotalPrice.Value;
            if(await _orderService.UpdateOrderAsync(id,dto))
            {
                return Ok(dto);
            }
            else
            {
                return StatusCode(500, "update errer");
            }
        }
        [HttpHead("{id}", Name = "ExistsOrderAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ExistsOrderAsync(int id) {
            var exist = await _orderService.ExistsOrderAsync(id);
            if (exist)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("GetOrdersByUserId",Name = "GetOrdersByUserIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrdersByUserIdAsync(int userId)
        {
            if (userId < 1)
            {
                return BadRequest("invalid data");
            }
            var OrdersByUserId = await _orderService.GetOrdersByUserIdAsync(userId);
            if(OrdersByUserId==null || !OrdersByUserId.Any())
            {
                return NotFound("order not found");
            }
            return Ok(OrdersByUserId);
        }
        [HttpPatch("{id}/status", Name = "UpdateOrderStatusAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateOrderStatusAsync(int id, string newStatus)
        {
            if(id<1 || string.IsNullOrEmpty(newStatus))
            {
                return BadRequest("Invalid Order Data");
            }
           if(await _orderService.UpdateOrderStatusAsync(id, newStatus)){
                return Ok();
            }
            else
            {
                return StatusCode(500, "update error");
            }

        }
    }
}
