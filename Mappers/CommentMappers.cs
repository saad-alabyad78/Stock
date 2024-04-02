using api.Dtos.Comment;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id ,
                Title = comment.Title ,
                Content = comment.Content ,
                CreatedOn = comment.CreatedOn ,
                StockId = comment.StockId
            };
        }

        public static Comment ToCommentFromCreateDto(this CreateCommetRequestDto comment,int stockId)
        {
            return new Comment
            {
                Title = comment.Title ,
                Content = comment.Content ,
                StockId = stockId
            };
        }
        public static Comment ToCommentFromUpdateDto(this UpdateCommentRequestDto comment)
        {
            return new Comment
            {
                Title = comment.Title ,
                Content = comment.Content ,
            };
        }
    }
}