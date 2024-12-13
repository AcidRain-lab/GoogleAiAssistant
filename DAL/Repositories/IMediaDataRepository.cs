using DAL.Models;

namespace DAL.Repositories
{
    public interface IMediaDataRepository
    {
        Task<List<MediaDatum>> GetMediaDataListByAssociatedRecordId(Guid associatedRecordId);
        Task<bool> SaveMediaDataList(List<MediaDatum> inputList);
    }
}
