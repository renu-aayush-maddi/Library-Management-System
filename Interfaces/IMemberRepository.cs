using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces;

public interface IMemberRepository
{
    void AddMember(Member member);

    List<Member> GetAllMembers();

    Member? GetMemberById(int memberId);

    void UpdateMember(Member member);

    Member? SearchMember(string keyword);

    void DeactivateMember(Member member);
}